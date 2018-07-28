using System;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.Emulators
{
  // TODO: consider moving this into a separate assembly

  /// <summary>Static extensions for service registrations.</summary>
  public static class ServiceCollectionExtensions
  {
    /// <summary>Adds an <see cref="IAmazonServiceEmulator{TService}"/> to the collection and configures it's <see cref="IAmazonServiceEmulator{TService}.Client"/>.</summary>
    public static IServiceCollection AddEmulator<TService, TEmulator>(this IServiceCollection services)
      where TService : class, IAmazonService
      where TEmulator : class, IAmazonServiceEmulator<TService>, new()
    {
      return AddEmulator<TService, TEmulator>(services, _ => new TEmulator());
    }

    /// <summary>Adds an <see cref="IAmazonServiceEmulator{TService}"/> to the collection and configures it's <see cref="IAmazonServiceEmulator{TService}.Client"/>.</summary>
    public static IServiceCollection AddEmulator<TClient, TEmulator>(this IServiceCollection services, Func<IServiceProvider, TEmulator> emulatorFactory)
      where TClient : class, IAmazonService
      where TEmulator : class, IAmazonServiceEmulator<TClient>
    {
      services.AddSingleton(emulatorFactory);
      services.AddSingleton(provider => provider.GetRequiredService<TEmulator>().Client);

      return services;
    }
  }
}