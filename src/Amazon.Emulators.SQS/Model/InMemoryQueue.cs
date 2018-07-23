using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Amazon.SQS.Model
{
  /// <summary>An in-memory SQS <see cref="IQueue"/> implementation</summary>
  /// <remarks>This implementation is thread-safe.</remarks>
  public sealed class InMemoryQueue : IQueue
  {
    private readonly ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

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

      messages.Enqueue(message);

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

          yield return message;
        }
      }

      return Take().ToArray();
    }
  }
}