using Amazon.SQS.Model;
using Xunit;

namespace Amazon.SQS
{
  public class AmazonSQSEmulatorTests
  {
    private readonly AmazonSQSEmulator emulator = new(RegionEndpoint.APSoutheast2, 123456789, url => new FileSystemQueue(url, basePath: "./queues"));

    [Fact]
    public void it_should_get_or_create_queues_via_url()
    {
      var queue1 = emulator.GetOrCreateByUrl("https://sqs.ap-southeast-2.amazonaws.com/123456789/test-queue");
      var queue2 = emulator.GetOrCreateByUrl("https://sqs.ap-southeast-2.amazonaws.com/123456789/test-queue");

      Assert.NotNull(queue1);
      Assert.NotNull(queue2);
      Assert.Equal(queue1, queue2);
    }
  }
}