using System;
using Amazon.Emulators.Embedded;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Amazon.Emulators.Example.Internal
{
  internal static class ServiceCollectionExtensions
  {
    public static IServiceCollection ReplaceWithEmbedded<TService, TEmbedded>(this IServiceCollection services)
      where TService : class, IAmazonService
      where TEmbedded : class, IEmbeddedAmazonService<TService>
    {
      services.RemoveAll<TService>();

      services.AddSingleton<TEmbedded>();
      services.AddSingleton(_ => _.GetRequiredService<TEmbedded>().Client);

      return services;
    }

    public static IServiceCollection ReplaceWithEmbedded<TService, TEmbedded>(this IServiceCollection services, Func<IServiceProvider, TEmbedded> factory)
      where TService : class, IAmazonService
      where TEmbedded : class, IEmbeddedAmazonService<TService>
    {
      Check.NotNull(factory, nameof(factory));

      services.RemoveAll<TService>();

      services.AddSingleton(factory);
      services.AddSingleton(_ => _.GetRequiredService<TEmbedded>().Client);

      return services;
    }
  }
}