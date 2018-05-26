using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace Amazon.Emulators
{
  public interface IAmazonServiceEmulatorHost<TService> : IDisposable
    where TService : class, IAmazonService
  {
    Task Run(CancellationToken cancellationToken = default);
  }
}