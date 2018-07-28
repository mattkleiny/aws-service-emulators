using System;
using TinyIpc.Messaging;

namespace Amazon.SQS.Model
{
  /// <summary>An inter-process SQS <see cref="IQueue"/> implementation</summary>
  public sealed class InterProcessQueue : IQueue, IDisposable
  {
    private readonly TinyMessageBus messageBus;

    public InterProcessQueue(QueueUrl queueUrl, string channelName)
    {
      Check.NotNull(queueUrl, nameof(queueUrl));
      Check.NotNullOrEmpty(channelName, nameof(channelName));

      Url         = queueUrl;
      ChannelName = channelName;

      messageBus = new TinyMessageBus($"{channelName}:{queueUrl}");
    }

    public QueueUrl Url         { get; }
    public string   ChannelName { get; }

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