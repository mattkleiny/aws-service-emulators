using Amazon.Emulators.Embedded;

namespace Amazon.S3
{
  /// <summary>An embedded implementation of <see cref="IAmazonS3"/>.</summary>
  public sealed class EmbeddedAmazonS3 : IEmbeddedAmazonService<IAmazonS3>
  {
    public IAmazonS3 Client { get; }
  }
}