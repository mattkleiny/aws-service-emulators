using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Embedded;
using Amazon.SimpleNotificationService;

namespace Amazon.SNS
{
  /// <summary>An embedded implementation of <see cref="IAmazonSimpleNotificationService"/>.</summary>
  public sealed class EmbeddedSNS : BackgroundEmbeddedService<IAmazonSimpleNotificationService>
  {
    public override IAmazonSimpleNotificationService Client { get; }

    protected override async Task WorkAsync(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        // TODO: dispatch queued notifications

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
      }
    }
  }
}