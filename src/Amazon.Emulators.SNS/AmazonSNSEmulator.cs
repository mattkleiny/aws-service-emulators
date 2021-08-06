using System;
using Amazon.Emulators;
using Amazon.SimpleNotificationService;
using Amazon.SNS.Internal;

namespace Amazon.SNS
{
  /// <summary>An emulator for Amazon's Simple Notification Service.</summary>
  public sealed class AmazonSNSEmulator : IAmazonServiceEmulator<IAmazonSimpleNotificationService>
  {
    public AmazonSNSEmulator()
    {
      Client = new EmulatedAmazonSNS(this);
    }

    /// <summary>Invoked when a new notification is published to the emulator.</summary>
    public event Action<Notification> NotificationPublished;

    public IAmazonSimpleNotificationService Client { get; }

    /// <summary>Publishes a new notification to the emulator.</summary>
    public void PublishNotification(string topicArn, string message, string subject)
    {
      NotificationPublished?.Invoke(new Notification(topicArn, message, subject));
    }
  }
}