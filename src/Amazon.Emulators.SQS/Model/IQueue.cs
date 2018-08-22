namespace Amazon.SQS.Model
{
  /// <summary>Represents a queue of <see cref="Message"/>s for use in our embedded SQS implementation.</summary>
  public interface IQueue
  {
    /// <summary>The number of items ready in the queue.</summary>
    int ReadyCount { get; }

    /// <summary>The number of items in flight from the queue.</summary>
    int InFlightCount { get; }

    /// <summary>The <see cref="QueueUrl"/> of this queue.</summary>
    QueueUrl Url { get; }

    /// <summary>Enqueues the given message and returns it's sequence number.</summary>
    long Enqueue(Message message);

    /// <summary>Dequeues a batch of up to <see cref="count"/> messages.</summary>
    Message[] Dequeue(int count);
  }
}