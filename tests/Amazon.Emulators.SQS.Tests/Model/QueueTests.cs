using System;
using Xunit;

namespace Amazon.SQS.Model
{
  public class QueueTests
  {
    [Fact]
    public void it_should_provide_a_valid_queue_url()
    {
      var queue = new Queue("test");
      var uri   = new Uri(queue.Url);

      Assert.NotNull(uri);
    }

    [Fact]
    public void it_should_queue_and_dequeue_correctly()
    {
      var queue = new Queue("test");

      queue.Enqueue(new Message());
      queue.Enqueue(new Message());
      queue.Enqueue(new Message());
      queue.Enqueue(new Message());

      var messages = queue.Dequeue(4);

      Assert.Equal(4, messages.Count);
    }
  }
}