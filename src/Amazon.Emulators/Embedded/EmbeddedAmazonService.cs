using System;
using Amazon.Runtime;

namespace Amazon.Emulators.Embedded
{
  public abstract class EmbeddedAmazonService : IAmazonService, IDisposable
  {
    private bool isDisposed;

    public IClientConfig Config { get; }

    public void Dispose()
    {
      if (isDisposed) throw new ObjectDisposedException(GetType().Name);

      Dispose(true);
      isDisposed = true;
    }

    protected virtual void Dispose(bool managed)
    {
    }
  }
}