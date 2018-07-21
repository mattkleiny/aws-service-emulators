using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Amazon.SQS.Model
{
  /// <summary>An embedded SQS queue.</summary>
  internal sealed class Queue
  {
    private readonly ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

    public Queue(string name)
    {
      Name = name;
    }

    public string Name { get; }
    public string Url  => $"https://embedded.amazon.sqs.com/123456789/{Name}";

    public void Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));

      messages.Enqueue(message);
    }

    public List<Message> Dequeue(int maxMessages)
    {
      Check.That(maxMessages > 0, "maxMessages > 0");

      return Take(maxMessages).ToList();
    }

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