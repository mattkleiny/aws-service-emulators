using Xunit;

namespace Amazon.SQS.Model
{
  public class InMemoryQueueTests
  {
    [Fact]
    public void it_should_provide_a_valid_queue_url_by_default()
    {
      var queue = BuildTestQueue();

      Assert.NotNull(queue.Url);
    }

    [Fact]
    public void it_should_queue_and_dequeue_correctly()
    {
      var queue = BuildTestQueue();

      queue.Enqueue(new Message());
      queue.Enqueue(new Message());
      queue.Enqueue(new Message());
      queue.Enqueue(new Message());

      var batch = queue.Dequeue(4);

      Assert.Equal(4, batch.Length);
    }

    private static InMemoryQueue BuildTestQueue() => new InMemoryQueue(new QueueUrl(RegionEndpoint.APSoutheast2, 123456789, "test-queue"));
  }
}