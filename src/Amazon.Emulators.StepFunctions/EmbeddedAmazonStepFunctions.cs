using Amazon.Emulators.Embedded;

namespace Amazon.StepFunctions
{
  /// <summary>An embedded implementation of <see cref="IAmazonStepFunctions"/>.</summary>
  public sealed class EmbeddedAmazonStepFunctions : IEmbeddedAmazonService<IAmazonStepFunctions>
  {
    public IAmazonStepFunctions Client { get; }
  }
}