using Amazon.Emulators.Embedded;

namespace Amazon.SQS
{
  /// <summary>An embedded implementation of <see cref="IAmazonSQS"/>.</summary>
  public sealed class EmbeddedSQS : EmbeddedService<IAmazonSQS>
  {
    public override IAmazonSQS Client { get; }
  }
}