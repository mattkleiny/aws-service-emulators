using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Amazon.SQS.Model
{
  /// <summary>An in-memory SQS <see cref="IQueue"/> implementation</summary>
  /// <remarks>This implementation is thread-safe.</remarks>
  internal sealed class InMemoryQueue : IQueue
  {
    private readonly ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

    public InMemoryQueue(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      Name = name;
      Url  = new Uri($"https://embedded.amazon.sqs.com/123456789/{Name}");
    }

    public string Name { get; }
    public Uri    Url  { get; }

    public void Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));

      messages.Enqueue(message);
    }

    public Message[] Dequeue(int maxMessages)
    {
      Check.That(maxMessages > 0, "maxMessages > 0");

      return Take(maxMessages).ToArray();
    }

    /// <summary>Tries to take up to <see cref="maxMessages"/> from the queue.</summary>
    private IEnumerable<Message> Take(int maxMessages)
    {
      for (var i = 0; i < maxMessages; i++)
      {
        if (!messages.TryDequeue(out var message))
        {
          yield break;
        }

        yield return message;
      }
    }
  }
}