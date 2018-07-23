using Xunit;

namespace Amazon.SQS.Model
{
  public class InMemoryQueueTests
  {
    [Fact]
    public void it_should_provide_a_valid_queue_url_by_default()
    {
      var queue = new InMemoryQueue("test");

      Assert.NotNull(queue.Url);
    }

    [Fact]
    public void it_should_queue_and_dequeue_correctly()
    {
      var queue = new InMemoryQueue("test");

      queue.Enqueue(new Message());
      queue.Enqueue(new Message());
      queue.Enqueue(new Message());
      queue.Enqueue(new Message());

      var batch = queue.Dequeue(4);

      Assert.Equal(4, batch.Length);
    }
  }
}