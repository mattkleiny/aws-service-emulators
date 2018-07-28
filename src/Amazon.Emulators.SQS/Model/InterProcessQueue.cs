using System;
using TinyIpc.Messaging;

namespace Amazon.SQS.Model
{
  /// <summary>An inter-process SQS <see cref="IQueue"/> implementation</summary>
  /// <remarks>This implementation is thread-safe.</remarks>
  public sealed class InterProcessQueue : IQueue, IDisposable
  {
    private readonly InMemoryQueue  innerQueue;
    private          TinyMessageBus messageBus;

    public InterProcessQueue(QueueUrl queueUrl)
    {
      Check.NotNull(queueUrl, nameof(queueUrl));

      innerQueue = new InMemoryQueue(queueUrl);
      messageBus = new TinyMessageBus(queueUrl.ToString());
    }

    public QueueUrl Url => innerQueue.Url;

    public long Enqueue(Message message)
    {
      return innerQueue.Enqueue(message);
    }

    public Message[] Dequeue(int count)
    {
      return innerQueue.Dequeue(count);
    }

    public void Dispose()
    {
      messageBus.Dispose();
    }
  }
}