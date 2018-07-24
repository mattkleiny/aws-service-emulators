using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Emulators.Workers
{
  /// <summary>Permits dispatching of <see cref="NotifyingBackgroundService.Notification"/>s to listeners. </summary>
  public delegate Task NotificationDispatcher(NotifyingBackgroundService.Notification notification, CancellationToken cancellationToken = default);

  /// <summary>A <see cref="BackgroundServiceBase"/> that supports a notification process.</summary>
  public abstract class NotifyingBackgroundService : BackgroundServiceBase
  {
    private readonly BlockingCollection<Notification> notifications = new BlockingCollection<Notification>();
    private readonly NotificationDispatcher           dispatcher;

    protected NotifyingBackgroundService(NotificationDispatcher dispatcher)
    {
      Check.NotNull(dispatcher, nameof(dispatcher));

      this.dispatcher = dispatcher;
    }

    /// <summary>Enqueues a <see cref="Notification"/> for dispatch.</summary>
    protected void EnqueueNotification(Notification notification)
    {
      Check.NotNull(notification, nameof(notification));

      notifications.Add(notification);
    }

    protected sealed override async Task WorkAsync(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        var notification = notifications.Take(cancellationToken);

        await dispatcher(notification, cancellationToken);
      }
    }

    /// <summary>Encapsulates a notification to be dispatched by the service.</summary>
    public sealed class Notification
    {
      public DateTime SendTime { get; } = DateTime.Now;
      public object   Payload  { get; set; }
    }
  }
}