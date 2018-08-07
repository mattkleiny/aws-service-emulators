using System;
using Amazon.Lambda;
using Amazon.Lambda.Hosting;
using Amazon.StepFunction.Hosting;

namespace Amazon.Emulators.Example
{
  public static class ServiceProviderExtensions
  {
    /// <summary>Builds a <see cref="LambdaResolver"/> from the given <see cref="IServiceProvider"/>.</summary>
    public static LambdaResolver ToLambdaResolver(this IServiceProvider provider)
      => (input, context) => provider.ResolveLambdaHandler(input, context).ExecuteAsync;

    /// <summary>Builds a <see cref="StepHandlerFactory"/> from the given <see cref="IServiceProvider"/>.</summary>
    public static StepHandlerFactory ToStepHandlerFactory(this IServiceProvider provider) => definition =>
    {
      var resolver = provider.ToLambdaResolver();
      var context  = new LocalLambdaContext(definition.Resource);

      return (input, cancellationToken) => resolver(input, context)(input, context, cancellationToken);
    };
  }
}