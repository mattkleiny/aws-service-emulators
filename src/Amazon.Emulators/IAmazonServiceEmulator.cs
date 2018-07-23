using Amazon.Runtime;

namespace Amazon.Emulators
{
  /// <summary>Represents a component which exposes a service client for the given Amazon <see cref="TService"/>.</summary>
  public interface IAmazonServiceEmulator<out TService>
    where TService : IAmazonService
  {
    /// <summary>The <see cref="TService"/> client for this provider.</summary>
    TService Client { get; }
  }
}