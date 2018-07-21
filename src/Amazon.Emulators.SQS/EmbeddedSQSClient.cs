using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Amazon.SQS
{
  /// <summary>An <see cref="IAmazonSQS"/> client that uses our <see cref="EmbeddedSQS"/> implementation.</summary>
  internal sealed class EmbeddedSQSClient : EmbeddedSQSClientBase
  {
    private readonly EmbeddedSQS parent;

    public EmbeddedSQSClient(EmbeddedSQS parent)
    {
      Check.NotNull(parent, nameof(parent));

      this.parent = parent;
    }

    public override Task<GetQueueUrlResponse> GetQueueUrlAsync(GetQueueUrlRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var queue = parent.GetOrCreateQueue(request.QueueName);

      return Task.FromResult(new GetQueueUrlResponse
      {
        QueueUrl       = queue.Url,
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      if (!parent.TryGetQueueByUrl(request.QueueUrl, out var queue))
      {
        return Task.FromResult(new SendMessageResponse
        {
          HttpStatusCode = HttpStatusCode.NotFound
        });
      }

      var message = new Message
      {
        MessageId  = Guid.NewGuid().ToString(),
        Body       = request.MessageBody,
        Attributes = request.MessageAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.StringValue)
      };

      queue.Enqueue(message);

      return Task.FromResult(new SendMessageResponse
      {
        MessageId      = message.MessageId,
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      if (!parent.TryGetQueueByUrl(request.QueueUrl, out var queue))
      {
        return Task.FromResult(new ReceiveMessageResponse
        {
          HttpStatusCode = HttpStatusCode.NotFound
        });
      }

      var messages = queue.Dequeue(request.MaxNumberOfMessages);

      return Task.FromResult(new ReceiveMessageResponse
      {
        Messages       = messages,
        HttpStatusCode = HttpStatusCode.OK
      });
    }
  }
}