using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Amazon.SQS.Internal
{
  // TODO: double check these response codes

  /// <summary>An <see cref="IAmazonSQS"/> implementation that delegates directly to an <see cref="AmazonSQSEmulator"/>.</summary>
  internal sealed class EmulatedAmazonSQS : AmazonSQSBase
  {
    private readonly AmazonSQSEmulator emulator;

    public EmulatedAmazonSQS(AmazonSQSEmulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }

    public override Task<GetQueueUrlResponse> GetQueueUrlAsync(GetQueueUrlRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var queue = emulator.GetOrCreateByName(request.QueueName);

      return Task.FromResult(new GetQueueUrlResponse
      {
        QueueUrl       = queue.Url.ToString(),
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var queue = emulator.GetOrCreateByUrl(request.QueueUrl);

      var message = new Message
      {
        MessageId  = Guid.NewGuid().ToString(),
        Body       = request.MessageBody,
        Attributes = request.MessageAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.StringValue)
      };

      var sequenceNumber = queue.Enqueue(message);

      return Task.FromResult(new SendMessageResponse
      {
        SequenceNumber = sequenceNumber.ToString(),
        MessageId      = message.MessageId,
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      // TODO: support receipt handles (perhaps use an intermediate struct?)

      var queue    = emulator.GetOrCreateByUrl(request.QueueUrl);
      var messages = queue.Dequeue(request.MaxNumberOfMessages);

      return Task.FromResult(new ReceiveMessageResponse
      {
        Messages       = messages.ToList(),
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override Task<GetQueueAttributesResponse> GetQueueAttributesAsync(GetQueueAttributesRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var queue = emulator.GetOrCreateByUrl(request.QueueUrl);

      return Task.FromResult(new GetQueueAttributesResponse
      {
        HttpStatusCode = HttpStatusCode.OK,
        Attributes = new Dictionary<string, string>
        {
          [nameof(GetQueueAttributesResponse.ApproximateNumberOfMessages)] = queue.ReadyCount.ToString()
        }
      });
    }
  }
}