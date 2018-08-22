using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Amazon.SQS.Model
{
  /// <summary>A <see cref="IQueue"/> implemented on the local file system.</summary>
  public sealed class FileSystemQueue : IQueue
  {
    private long sequence;

    public FileSystemQueue(QueueUrl url, string basePath)
    {
      Check.NotNull(url, nameof(url));
      Check.NotNullOrEmpty(basePath, nameof(basePath));

      Url      = url;
      BasePath = basePath;

      ReadyPath    = Path.Combine(basePath, url.Name, "ready");
      InFlightPath = Path.Combine(basePath, url.Name, "in-flight");

      CreateDirectoryIfRequired(ReadyPath);
      CreateDirectoryIfRequired(InFlightPath);
    }

    public QueueUrl Url          { get; }
    public string   BasePath     { get; }
    public string   ReadyPath    { get; }
    public string   InFlightPath { get; }

    public int ReadyCount    => Directory.GetFiles(ReadyPath).Length;
    public int InFlightCount => Directory.GetFiles(InFlightPath).Length;

    public long Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));

      // create a message id, if one is not provided
      if (string.IsNullOrEmpty(message.MessageId))
      {
        message.MessageId = Guid.NewGuid().ToString();
      }

      // use the message id plus the sequence number as the receipt handle
      var seq = Interlocked.Increment(ref sequence);
      message.ReceiptHandle = $"{seq}-{message.MessageId}";

      var serialized = JsonConvert.SerializeObject(message);
      var path       = Path.Combine(ReadyPath, message.ReceiptHandle);

      File.WriteAllText(path, serialized, Encoding.UTF8);

      return seq;
    }

    public Message[] Dequeue(int count)
    {
      Check.That(count >= 0, "count >= 0");

      // TODO: transition old in-flight items back to the primary queue
      
      var results = new List<Message>(count);
      var files   = Directory.GetFiles(ReadyPath).Take(count);

      foreach (var file in files)
      {
        results.Add(JsonConvert.DeserializeObject<Message>(File.ReadAllText(file)));

        MarkAsInFlight(file);
      }

      return results.ToArray();
    }

    public void Delete(string handle)
    {
      Check.NotNullOrEmpty(handle, nameof(handle));

      var path = Path.Combine(InFlightPath, handle);
      
      if (File.Exists(path))
      {
        File.Delete(path);
      }
    }

    private static void CreateDirectoryIfRequired(string path)
    {
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
    }

    private void MarkAsInFlight(string file) => File.Move(file, Path.Combine(InFlightPath, Path.GetFileName(file)));
  }
}