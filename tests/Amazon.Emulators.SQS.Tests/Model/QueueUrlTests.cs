using Xunit;

namespace Amazon.SQS.Model
{
  public class QueueUrlTests
  {
    [Theory]
    [InlineData("https://sqs.ap-southeast-2.amazonaws.com/123456789/test-queue-10")]
    [InlineData("http://sqs.ap-southeast-2.amazonaws.com/123456789/test-queue-12")]
    [InlineData("https://sqs.us-east-1.amazonaws.com/123456789/test-queue-59")]
    [InlineData("http://sqs.us-east-1.amazonaws.com/123456789/test-queue-32")]
    public void it_should_parse_simple_urls(string url) => QueueUrl.Parse(url);
  }
}