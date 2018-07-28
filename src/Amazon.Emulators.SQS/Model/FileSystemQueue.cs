using System;

namespace Amazon.SQS.Model
{
  /// <summary>A <see cref="IQueue"/> implemented on the local file system.</summary>
  public sealed class FileSystemQueue : IQueue
  {
    public FileSystemQueue(QueueUrl url, string rootPath)
    {
      Check.NotNull(url, nameof(url));
      Check.NotNullOrEmpty(rootPath, nameof(rootPath));

      Url      = url;
      RootPath = rootPath;
    }

    public QueueUrl Url      { get; }
    public string   RootPath { get; }

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