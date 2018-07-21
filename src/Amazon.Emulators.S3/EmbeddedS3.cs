using Amazon.Emulators.Embedded;

namespace Amazon.S3
{
  /// <summary>An embedded implementation of <see cref="IAmazonS3"/>.</summary>
  public sealed class EmbeddedS3 : EmbeddedService<IAmazonS3>
  {
    public override IAmazonS3 Client { get; }
  }
}