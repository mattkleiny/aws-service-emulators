using Amazon.Emulators.Embedded;

namespace Amazon.Lambda
{
  /// <summary>An embedded implementation of <see cref="IAmazonLambda"/>.</summary>
  public sealed class EmbeddedAmazonLambda : IEmbeddedAmazonService<IAmazonLambda>
  {
    public IAmazonLambda Client { get; }
  }
}