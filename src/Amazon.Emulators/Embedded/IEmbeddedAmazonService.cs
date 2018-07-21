using Amazon.Runtime;

namespace Amazon.Emulators.Embedded
{
  /// <summary>Represents an embedded <see cref="IAmazonService"/>.</summary>
  public interface IEmbeddedAmazonService<out TService>
    where TService : class, IAmazonService
  {
    /// <summary>A direct <see cref="!:TService" /> client implementation against the embedded component.</summary>
    TService Client { get; }
  }
}