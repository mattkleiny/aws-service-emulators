using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Newtonsoft.Json;

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
        MD5OfBody              = CalculateMD5(request.MessageBody),
        MD5OfMessageAttributes = CalculateMD5(request.MessageAttributes)
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

      // TODO: support receipt handles (perhaps use an intermediate struct?)

      var queue    = emulator.GetOrCreateByUrl(request.QueueUrl);
      var messages = queue.Dequeue(request.MaxNumberOfMessages);

      return Task.FromResult(new ReceiveMessageResponse
      {
        Messages       = messages.ToList(),
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    /// <summary>Converts the given object to JSON and calculates it's MD5 hash.</summary>
    private static string CalculateMD5(object input)
    {
      var json = JsonConvert.SerializeObject(input);

      return CalculateMD5(json);
    }

    /// <summary>Calculates the MD5 hash of the given string.</summary>
    private static string CalculateMD5(string input)
    {
      using (var md5 = MD5.Create())
      {
        var bytes = Encoding.ASCII.GetBytes(input);
        var hash  = md5.ComputeHash(bytes);

        var builder = new StringBuilder();

        for (var i = 0; i < bytes.Length; i++)
        {
          builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
      }
    }
  }
}