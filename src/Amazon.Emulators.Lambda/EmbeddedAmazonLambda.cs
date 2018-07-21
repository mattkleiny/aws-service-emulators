using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Embedded;
using Amazon.Lambda.Core;

namespace Amazon.Lambda
{
  public delegate LambdaHandler LambdaHandlerResolver(object input, ILambdaContext context);
  public delegate Task<object> LambdaHandler(object input, ILambdaContext context, CancellationToken cancellationToken = default);

  /// <summary>An embedded implementation of <see cref="IAmazonLambda"/>.</summary>
  public sealed class EmbeddedAmazonLambda : IEmbeddedAmazonService<IAmazonLambda>
  {
    private readonly LambdaHandlerResolver resolver;

    public EmbeddedAmazonLambda(LambdaHandlerResolver resolver)
    {
      Check.NotNull(resolver, nameof(resolver));

      this.resolver = resolver;

      Client = new EmbeddedLambdaClient(this);
    }

    public IAmazonLambda Client { get; }

    internal LambdaHandler ResolveLambdaHandler(object input, ILambdaContext context)
    {
      Check.NotNull(context, nameof(context));

      return resolver(input, context);
    }
  }
}