using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Amazon.SQS.Model
{
  // TODO: support visibility timeouts
  // TODO: support delivery timeouts
  // TODO: support receipt handles

  /// <summary>An in-memory SQS <see cref="IQueue"/> implementation</summary>
  public sealed class InMemoryQueue : IQueue
  {
    private readonly ConcurrentQueue<QueuedMessage> messages = new ConcurrentQueue<QueuedMessage>();

    private long sequenceNumber;

    public InMemoryQueue(QueueUrl url)
    {
      Check.NotNull(url, nameof(url));

      Url = url;
    }

    public QueueUrl Url { get; }

    public long Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));

      messages.Enqueue(QueuedMessage.From(message));

      return Interlocked.Increment(ref sequenceNumber);
    }

    public Message[] Dequeue(int count)
    {
      Check.That(count > 0, "maxMessages > 0");

      IEnumerable<Message> Take()
      {
        for (var i = 0; i < count; i++)
        {
          if (!messages.TryDequeue(out var message))
          {
            yield break;
          }

          yield return message.ToMessage();
        }
      }

      return Take().ToArray();
    }

    /// <summary>An in-memory representation of a message.</summary>
    private sealed class QueuedMessage
    {
      public static QueuedMessage From(Message message) => new QueuedMessage
      {
        MessageId              = message.MessageId,
        Body                   = message.Body,
        MD5OfBody              = message.MD5OfBody,
        MD5OfMessageAttributes = message.MD5OfMessageAttributes,
        Attributes             = message.Attributes.ToImmutableDictionary()
      };

      public string MessageId              { get; set; }
      public string Body                   { get; set; }
      public string MD5OfBody              { get; set; }
      public string MD5OfMessageAttributes { get; set; }

      public IImmutableDictionary<string, string> Attributes { get; set; }

      public Message ToMessage() => new Message
      {
        MessageId              = MessageId,
        Body                   = Body,
        MD5OfBody              = MD5OfBody,
        MD5OfMessageAttributes = MD5OfMessageAttributes,
        Attributes             = Attributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
        ReceiptHandle          = Guid.NewGuid().ToString()
      };
    }
  }
}