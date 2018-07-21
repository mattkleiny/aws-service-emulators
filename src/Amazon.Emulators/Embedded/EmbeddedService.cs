using Amazon.Runtime;

namespace Amazon.Emulators.Embedded
{
  /// <summary>Base class for any <see cref="IEmbeddedService{TService}"/> implementations.</summary>
  public abstract class EmbeddedService<TService> : IEmbeddedService<TService> 
    where TService : class, IAmazonService
  {
    public abstract TService Client { get; }
  }
}