using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;

namespace Amazon.SNS.Internal
{
  internal sealed class EmulatedAmazonSNS : AmazonSNSBase
  {
    private readonly AmazonSNSEmulator emulator;

    public EmulatedAmazonSNS(AmazonSNSEmulator emulator)
    {
      this.emulator = emulator;
    }

    public override Task<PublishResponse> PublishAsync(PublishRequest request, CancellationToken cancellationToken = default)
    {
      emulator.PublishNotification(request.TopicArn, request.Message, request.Subject);

      return Task.FromResult(new PublishResponse
      {
        HttpStatusCode = HttpStatusCode.OK
      });
    }
  }
}