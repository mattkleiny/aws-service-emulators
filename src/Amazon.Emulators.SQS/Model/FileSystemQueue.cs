using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Amazon.SQS.Model
{
  // TODO: give this a once over

  /// <summary>A <see cref="IQueue"/> implemented on the local file system.</summary>
  public sealed class FileSystemQueue : IQueue, IDisposable
  {
    private readonly object                  semaphore      = new();
    private readonly CancellationTokenSource backgroundTask = new();

    private long sequence;

    public FileSystemQueue(QueueUrl url, string basePath)
    {
      Check.NotNull(url, nameof(url));
      Check.NotNullOrEmpty(basePath, nameof(basePath));

      Url      = url;
      BasePath = basePath;

      WaitingPath  = Path.Combine(basePath, url.Name, "waiting");
      ReadyPath    = Path.Combine(basePath, url.Name, "ready");
      InFlightPath = Path.Combine(basePath, url.Name, "in-flight");

      CreateDirectoryIfRequired(WaitingPath);
      CreateDirectoryIfRequired(ReadyPath);
      CreateDirectoryIfRequired(InFlightPath);

      Task.Factory.StartNew(
        () => PerformMaintenanceAsync(backgroundTask.Token).Wait(),
        backgroundTask.Token,
        TaskCreationOptions.LongRunning,
        TaskScheduler.Default
      );
    }

    public string BasePath { get; }

    private string WaitingPath  { get; }
    private string ReadyPath    { get; }
    private string InFlightPath { get; }

    public TimeSpan DeliveryTimeout   { get; set; } = TimeSpan.Zero;
    public TimeSpan VisibilityTimeout { get; set; } = TimeSpan.MaxValue;
    public TimeSpan PollInterval      { get; set; } = TimeSpan.FromSeconds(5);

    public QueueUrl Url { get; }

    public int ReadyCount    => Directory.GetFiles(ReadyPath).Length;
    public int InFlightCount => Directory.GetFiles(InFlightPath).Length;

    public long Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));

      // determine which bucket to put the message into
      string GetTargetPath()
      {
        if (DeliveryTimeout > TimeSpan.Zero)
        {
          return Path.Combine(WaitingPath, message.ReceiptHandle);
        }

        return Path.Combine(ReadyPath, message.ReceiptHandle);
      }

      lock (semaphore)
      {
        // create a message id, if one is not provided
        if (string.IsNullOrEmpty(message.MessageId))
        {
          message.MessageId = Guid.NewGuid().ToString();
        }

        // the receipt handle is just the file name, prefixed with the sequence number
        var seq = Interlocked.Increment(ref sequence);

        message.ReceiptHandle = $"{seq}-{message.MessageId}.json";

        var serialized = JsonConvert.SerializeObject(message);
        var path       = GetTargetPath();

        File.WriteAllText(path, serialized, Encoding.UTF8);

        return seq;
      }
    }

    public bool TryDequeue(out Message message)
    {
      lock (semaphore)
      {
        var file = Directory.GetFiles(ReadyPath).FirstOrDefault();

        if (string.IsNullOrEmpty(file))
        {
          message = null;
          return false;
        }

        var json = File.ReadAllText(file);
        message = JsonConvert.DeserializeObject<Message>(json);

        Move(file, InFlightPath);

        return true;
      }
    }

    public void Delete(string handle)
    {
      Check.NotNullOrEmpty(handle, nameof(handle));

      lock (semaphore)
      {
        // try and delete either from in-flight or directly out of ready
        DeleteIfExists(Path.Combine(ReadyPath,    handle));
        DeleteIfExists(Path.Combine(InFlightPath, handle));
      }
    }

    public void Dispose() => backgroundTask.Dispose();

    /// <summary>Processes state transitions in the background.</summary>
    private async Task PerformMaintenanceAsync(CancellationToken cancellationToken)
    {
      void ScanAndTransitionOldMessages(string from, string to, TimeSpan after)
      {
        foreach (var file in Directory.GetFiles(from))
        {
          var duration = DateTime.Now - File.GetLastWriteTime(file);
          if (duration > after) Move(file, to);
        }
      }

      while (!cancellationToken.IsCancellationRequested)
      {
        lock (semaphore)
        {
          ScanAndTransitionOldMessages(from: WaitingPath,  to: ReadyPath, DeliveryTimeout);
          ScanAndTransitionOldMessages(from: InFlightPath, to: ReadyPath, VisibilityTimeout);
        }

        // don't eat the cpu
        await Task.Delay(PollInterval, cancellationToken);
      }
    }

    /// <summary>Creates the given directory, if it doesn't already exist.</summary>
    private static void CreateDirectoryIfRequired(string path)
    {
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
    }

    /// <summary>Deletes the given file, if it exists.</summary>
    private static void DeleteIfExists(string path)
    {
      if (File.Exists(path))
      {
        File.Delete(path);
      }
    }

    /// <summary>Moves a file with an absolute path to the given directory</summary>
    private static void Move(string from, string to) => File.Move(from, Path.Combine(to, Path.GetFileName(from)));
  }
}