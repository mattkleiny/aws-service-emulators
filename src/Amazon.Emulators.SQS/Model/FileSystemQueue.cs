using System;

namespace Amazon.SQS.Model
{
  /// <summary>A <see cref="IQueue"/> implemented on the local file system.</summary>
  public sealed class FileSystemQueue : IQueue
  {
    public FileSystemQueue(QueueUrl url, string basePath)
    {
      Check.NotNull(url, nameof(url));
      Check.NotNullOrEmpty(basePath, nameof(basePath));

      Url      = url;
      BasePath = basePath;
    }

    public int      Count    { get; }
    public QueueUrl Url      { get; }
    public string   BasePath { get; }

    public long Enqueue(Message message)
    {
      throw new NotImplementedException();
    }

    public Message[] Dequeue(int count)
    {
      throw new NotImplementedException();
    }
  }
}