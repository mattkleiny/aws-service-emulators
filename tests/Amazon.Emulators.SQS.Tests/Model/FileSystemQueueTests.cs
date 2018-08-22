using Newtonsoft.Json;
using Xunit;

namespace Amazon.SQS.Model
{
  public class FileSystemQueueTests
  {
    [Fact]
    public void it_should_write_to_disk_read_back_and_delete_without_fault()
    {
      var url   = new QueueUrl(RegionEndpoint.APSoutheast2, 123456789, "test");
      var queue = new FileSystemQueue(url, basePath: "./queues");

      for (var i = 0; i < 5; i++)
      {
        queue.Enqueue(new Message
        {
          Body = JsonConvert.SerializeObject(new
          {
            Message = "Hello, World!"
          })
        });
      }

      var messages = queue.Dequeue(5);
      
      foreach (var message in messages)
      {
        queue.Delete(message.ReceiptHandle);
      }
    }
  }
}