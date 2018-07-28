using Amazon.Runtime;

namespace Amazon.Emulators
{
  /// <summary>Represents a component which exposes a service client for the given Amazon <see cref="TClient"/>.</summary>
  public interface IAmazonServiceEmulator<out TClient>
    where TClient : IAmazonService
  {
    /// <summary>The <see cref="TClient"/> client for this provider.</summary>
    TClient Client { get; }
  }
}