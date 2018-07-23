using System;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.Emulators
{
  // TODO: consider moving this into a separate assembly

  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddEmulator<TService, TEmulator>(this IServiceCollection services)
      where TService : class, IAmazonService
      where TEmulator : class, IAmazonServiceEmulator<TService>, new()
    {
      return AddEmulator<TService, TEmulator>(services, () => new TEmulator());
    }

    public static IServiceCollection AddEmulator<TService, TEmulator>(this IServiceCollection services, Func<TEmulator> emulatorFactory)
      where TService : class, IAmazonService
      where TEmulator : class, IAmazonServiceEmulator<TService>
    {
      services.AddSingleton(_ => emulatorFactory());
      services.AddSingleton(provider => provider.GetRequiredService<TEmulator>().Client);

      return services;
    }
  }
}