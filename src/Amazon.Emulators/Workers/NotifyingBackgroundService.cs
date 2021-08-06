using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Emulators.Workers
{
  /// <summary>Permits dispatching of <see cref="NotifyingBackgroundService.Notification"/>s to listeners. </summary>
  public delegate Task NotificationDispatcher(object payload, CancellationToken cancellationToken = default);

  /// <summary>A <see cref="BackgroundServiceBase"/> that supports a notification process.</summary>
  public abstract class NotifyingBackgroundService : BackgroundServiceBase
  {
    private readonly BlockingCollection<Notification> notifications = new();
    private readonly NotificationDispatcher           dispatcher;

    private long sequence;

    protected NotifyingBackgroundService(NotificationDispatcher dispatcher)
    {
      Check.NotNull(dispatcher, nameof(dispatcher));

      this.dispatcher = dispatcher;
    }

    /// <summary>Enqueues a notification for dispatch.</summary>
    protected void Enqueue(object payload)
    {
      Check.NotNull(payload, nameof(payload));

      notifications.Add(new Notification
      {
        Id       = Interlocked.Increment(ref sequence),
        SendTime = DateTime.Now,
        Payload  = payload
      });
    }

    protected sealed override async Task WorkAsync(CancellationToken cancellationToken)
    {
      foreach (var notification in notifications.GetConsumingEnumerable(cancellationToken))
      {
        await dispatcher(notification.Payload, cancellationToken);
      }
    }

    /// <summary>Encapsulates a notification to be dispatched by the service.</summary>
    private sealed class Notification
    {
      public long     Id       { get; set; }
      public DateTime SendTime { get; set; }
      public object   Payload  { get; set; }
    }
  }
}