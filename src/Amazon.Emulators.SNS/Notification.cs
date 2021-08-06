using System;

namespace Amazon.SNS
{
  /// <summary>A wrapper for a notification published by some source.</summary>
  public readonly struct Notification : IEquatable<Notification>
  {
    public string         TopicArn    { get; }
    public string         Message     { get; }
    public string         Subject     { get; }
    public DateTimeOffset PublishedAt { get; }

    public Notification(string topicArn, string message, string subject)
    {
      TopicArn    = topicArn;
      Message     = message;
      Subject     = subject;
      PublishedAt = DateTimeOffset.Now;
    }

    public bool Equals(Notification other)
    {
      return TopicArn == other.TopicArn &&
             Message  == other.Message  &&
             Subject  == other.Subject  &&
             PublishedAt.Equals(other.PublishedAt);
    }

    public override bool Equals(object obj)
    {
      return obj is Notification other && Equals(other);
    }

    public override int GetHashCode() => HashCode.Combine(TopicArn, Message, Subject, PublishedAt);

    public static bool operator ==(Notification left, Notification right) => left.Equals(right);
    public static bool operator !=(Notification left, Notification right) => !left.Equals(right);
  }
}