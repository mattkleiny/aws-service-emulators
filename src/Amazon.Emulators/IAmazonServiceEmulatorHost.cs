using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Embedded;

namespace Amazon.Emulators
{
  /// <summary>Permits hosting of an <see cref="EmbeddedAmazonService"/> via an HTTP server.</summary>
  public sealed class AmazonServiceEmulatorHost<TService> : IDisposable
    where TService : EmbeddedAmazonService, new()
  {
    public AmazonServiceEmulatorHost(int port)
    {
      Port = port;
    }

    public int Port { get; }

    /// <summary>Executes the emulator host asynchronusly and returns a task representing it's execution.</summary>
    public Task Run(CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
    }
  }
}