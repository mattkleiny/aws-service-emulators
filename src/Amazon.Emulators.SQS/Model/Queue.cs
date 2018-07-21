using System.Collections.Concurrent;

namespace Amazon.SQS.Model
{
  /// <summary>An embedded SQS queue.</summary>
  internal sealed class Queue
  {
    private readonly ConcurrentQueue<Message> messages = new ConcurrentQueue<Message>();

    public string Url { get; set; }

    public void Enqueue(Message message)
    {
      Check.NotNull(message, nameof(message));

      messages.Enqueue(message);
    }
  }
}