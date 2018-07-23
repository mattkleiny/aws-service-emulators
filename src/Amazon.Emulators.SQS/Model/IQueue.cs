using System;

namespace Amazon.SQS.Model
{
  /// <summary>Represents a queue of <see cref="Message"/>s for use in our embedded SQS implementation.</summary>
  internal interface IQueue
  {
    /// <summary>The name of this queue.</summary>
    string Name { get; }

    /// <summary>The URL of this queue.</summary>
    Uri Url { get; }

    /// <summary>Enqueues the given message.</summary>
    void Enqueue(Message message);

    /// <summary>Dequeues a batch of up to <see cref="maxMessages"/> messages.</summary>
    Message[] Dequeue(int maxMessages);
  }
}