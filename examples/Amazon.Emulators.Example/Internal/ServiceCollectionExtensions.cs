using Amazon.Emulators.Embedded;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Amazon.Emulators.Example.Internal
{
  internal static class ServiceCollectionExtensions
  {
    public static IServiceCollection ReplaceWithEmbedded<TService, TImplementation>(this IServiceCollection services)
      where TService : class, IAmazonService
      where TImplementation : class, IEmbeddedService<TService>, new()
    {
      services.RemoveAll<TService>();
      
      services.AddSingleton<TImplementation>();
      services.AddSingleton(_ => _.GetRequiredService<TImplementation>().Client);

      return services;
    }
  }
}