using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using TinyIpc.Messaging;

namespace Amazon.SQS.Model
{
  /// <summary>An inter-process SQS <see cref="IQueue"/> implementation</summary>
  public sealed class InterProcessQueue : IQueue, IDisposable
  {
    private readonly IQueue         innerQueue;
    private readonly TinyMessageBus messageBus;

    public InterProcessQueue(string channelName, IQueue innerQueue)
    {
      Check.NotNullOrEmpty(channelName, nameof(channelName));
      Check.NotNull(innerQueue, nameof(innerQueue));

      this.innerQueue = innerQueue;
      ChannelName     = channelName;

      messageBus = new TinyMessageBus($"{channelName}:{innerQueue.Url}");

      messageBus.MessageReceived += OnMessageReceived;
    }

    public QueueUrl Url         => innerQueue.Url;
    public string   ChannelName { get; }

    public long Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));
      
      var envelope = new Envelope<Message>(message);

      messageBus.PublishAsync(envelope.ToBytes());
      
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

    private void OnMessageReceived(object sender, TinyMessageReceivedEventArgs eventArgs)
    {
      var envelope = Envelope<Message>.FromBytes(eventArgs.Message);

      innerQueue.Enqueue(envelope.Payload);
    }

    /// <summary>An envelope for inter-process communication.</summary>
    private sealed class Envelope<T>
      where T : class
    {
      public static Envelope<T> FromBytes(byte[] bytes)
      {
        Check.NotNull(bytes, nameof(bytes));

        var json = Encoding.UTF8.GetString(bytes);

        return JsonConvert.DeserializeObject<Envelope<T>>(json);
      }

      public Envelope(T payload)
      {
        Check.NotNull(payload, nameof(payload));

        Payload = payload;
      }

      public Guid Id        { get; } = Guid.NewGuid();
      public long ProcessId { get; } = Process.GetCurrentProcess().Id;
      public T    Payload   { get; }

      /// <summary>Converts the envelope to a series of bytes.</summary>
      public byte[] ToBytes()
      {
        var json = JsonConvert.SerializeObject(this);

        return Encoding.UTF8.GetBytes(json);
      }
    }
  }
}