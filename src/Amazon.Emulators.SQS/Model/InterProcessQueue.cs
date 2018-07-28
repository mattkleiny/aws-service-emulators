using System;
using TinyIpc.Messaging;

namespace Amazon.SQS.Model
{
  /// <summary>An inter-process SQS <see cref="IQueue"/> implementation</summary>
  /// <remarks>This implementation is thread-safe.</remarks>
  public sealed class InterProcessQueue : IQueue, IDisposable
  {
    private readonly TinyMessageBus messageBus;

    public InterProcessQueue(string channelName, QueueUrl queueUrl)
    {
      Check.NotNullOrEmpty(channelName, nameof(channelName));
      Check.NotNull(queueUrl, nameof(queueUrl));

      Url = queueUrl;

      messageBus = new TinyMessageBus($"{channelName}:{queueUrl}");
    }

    public QueueUrl Url { get; }

    public long Enqueue(Message message)
    {
      throw new NotImplementedException();
    }

    public Message[] Dequeue(int count)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      messageBus.Dispose();
    }
  }
}