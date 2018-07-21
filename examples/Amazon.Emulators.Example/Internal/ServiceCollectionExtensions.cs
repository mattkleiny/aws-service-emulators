using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Amazon.Emulators.Example.Internal
{
  internal static class ServiceCollectionExtensions
  {
    public static IServiceCollection ReplaceWith<TService, TImplementation>(this IServiceCollection services) 
      where TService : class 
      where TImplementation : class, TService
    {
      services.RemoveAll<TService>();
      services.AddSingleton<TService, TImplementation>();

      return services;
    }
  }
}