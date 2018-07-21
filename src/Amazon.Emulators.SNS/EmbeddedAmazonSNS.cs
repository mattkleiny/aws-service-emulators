using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Embedded;
using Amazon.SimpleNotificationService;

namespace Amazon.SNS
{
  public delegate Task NotificationHandler(CancellationToken cancellationToken);
  public delegate NotificationHandler NotificationHandlerFactory();

  /// <summary>An embedded implementation of <see cref="IAmazonSimpleNotificationService"/>.</summary>
  public sealed class EmbeddedAmazonSNS : BackgroundAmazonService<IAmazonSimpleNotificationService>
  {
    private readonly NotificationHandlerFactory factory;

    public EmbeddedAmazonSNS(NotificationHandlerFactory factory)
    {
      Check.NotNull(factory, nameof(factory));

      this.factory = factory;
    }

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