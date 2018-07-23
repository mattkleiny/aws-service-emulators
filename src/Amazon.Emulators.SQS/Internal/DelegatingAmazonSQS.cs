using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Amazon.SQS.Internal
{
  // TODO: double check these response codes

  /// <summary>An <see cref="IAmazonSQS"/> implementation that delegates directly to an <see cref="AmazonSQSEmulator"/>.</summary>
  internal sealed class DelegatingAmazonSQS : AmazonSQSBase
  {
    private readonly AmazonSQSEmulator emulator;

    public DelegatingAmazonSQS(AmazonSQSEmulator emulator)
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
        MessageId              = Guid.NewGuid().ToString(),
        Body                   = request.MessageBody,
        Attributes             = request.MessageAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.StringValue),
        MD5OfBody              = string.Empty, // TODO: calculate this
        MD5OfMessageAttributes = string.Empty  // TODO: calculate this
      };

      var sequenceNumber = queue.Enqueue(message);

      return Task.FromResult(new SendMessageResponse
      {
        SequenceNumber         = sequenceNumber.ToString(),
        MessageId              = message.MessageId,
        MD5OfMessageBody       = message.MD5OfBody,
        MD5OfMessageAttributes = message.MD5OfMessageAttributes,
        HttpStatusCode         = HttpStatusCode.OK
      });
    }

    public override Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var queue    = emulator.GetOrCreateByUrl(request.QueueUrl);
      var messages = queue.Dequeue(request.MaxNumberOfMessages);

      return Task.FromResult(new ReceiveMessageResponse
      {
        Messages       = messages.ToList(),
        HttpStatusCode = HttpStatusCode.OK
      });
    }
  }
}