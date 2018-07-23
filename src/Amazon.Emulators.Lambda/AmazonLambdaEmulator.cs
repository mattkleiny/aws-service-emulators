using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.Lambda.Core;
using Amazon.Lambda.Internal;
using Amazon.Lambda.Model;

namespace Amazon.Lambda
{
  /// <summary>An emulator for Amazon's Lambda Service.</summary>
  /// <remarks>This implementation is thread-safe.</remarks>
  public sealed class AmazonLambdaEmulator : IAmazonServiceEmulator<IAmazonLambda>
  {
    private readonly LambdaResolver resolver;

    public AmazonLambdaEmulator(LambdaResolver resolver)
    {
      Check.NotNull(resolver, nameof(resolver));

      this.resolver = resolver;

      Client = new DelegatingAmazonLambda(this);
    }

    public IAmazonLambda Client { get; }

    /// <summary>Resolves a <see cref="LambdaHandler"/> for the given context.</summary>
    public LambdaHandler ResolveHandler(object input, ILambdaContext context)
    {
      Check.NotNull(context, nameof(context));

      return resolver(input, context);
    }

    /// <summary>Resolves and executes a lambda for the given input and context.</summary>
    public async Task<object> ExecuteLambdaAsync(object input, ILambdaContext context, CancellationToken cancellationToken = default)
    {
      var handler = ResolveHandler(input, context);

      return await handler(input, context, cancellationToken);
    }
  }
}