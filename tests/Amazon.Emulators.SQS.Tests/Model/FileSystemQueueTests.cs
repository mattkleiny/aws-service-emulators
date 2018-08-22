using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Xunit;

namespace Amazon.SQS.Model
{
  public class FileSystemQueueTests
  {
    [Fact]
    public void it_should_write_to_disk_read_back_and_delete_without_fault()
    {
      var url = new QueueUrl(RegionEndpoint.APSoutheast2, 123456789, "test");
      var queue = new FileSystemQueue(url, basePath: "./queues")
      {
        VisibilityTimeout = TimeSpan.FromSeconds(10)
      };

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

      var messages = new List<Message>(5);
      
      while (queue.TryDequeue(out var message))
      {
        messages.Add(message);
      }
      
      Thread.Sleep(TimeSpan.FromSeconds(10));

      foreach (var message in messages)
      {
        queue.Delete(message.ReceiptHandle);
      }
    }
  }
}