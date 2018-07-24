using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Amazon.Emulators.Workers
{
  /// <summary>A <see cref="Microsoft.Extensions.Hosting.BackgroundService"/> specialization that supports a single long-running background worker.</summary>
  public abstract class BackgroundServiceBase : BackgroundService
  {
    public sealed override Task StartAsync(CancellationToken cancellationToken)
    {
      return base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
      return Task.Factory.StartNew(
        () => WorkAsync(cancellationToken).Wait(cancellationToken),
        cancellationToken,
        TaskCreationOptions.LongRunning,
        TaskScheduler.Default
      );
    }

    public sealed override Task StopAsync(CancellationToken cancellationToken)
    {
      return base.StopAsync(cancellationToken);
    }

    protected abstract Task WorkAsync(CancellationToken cancellationToken);
  }
}