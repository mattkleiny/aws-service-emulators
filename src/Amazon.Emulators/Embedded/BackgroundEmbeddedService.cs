using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Microsoft.Extensions.Hosting;

namespace Amazon.Emulators.Embedded
{
  /// <summary>A background <see cref="IEmbeddedService{TService}"/> that uses a long-running worker for background tasks.</summary>
  public abstract class BackgroundEmbeddedService<TService> : BackgroundService, IEmbeddedService<TService>
    where TService : class, IAmazonService
  {
    public abstract TService Client { get; }

    public sealed override Task StartAsync(CancellationToken cancellationToken)
    {
      return base.StartAsync(cancellationToken);
    }

    protected sealed override Task ExecuteAsync(CancellationToken cancellationToken)
    {
      return Task.Factory.StartNew(
        () => WorkAsync(cancellationToken),
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