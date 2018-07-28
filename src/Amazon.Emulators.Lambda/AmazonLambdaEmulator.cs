﻿using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.Lambda.Core;
using Amazon.Lambda.Internal;

namespace Amazon.Lambda
{
  /// <summary>Permits resolution of <see cref="LambdaHandler"/>s for some given input and context.</summary>
  public delegate LambdaHandler LambdaResolver(object input, ILambdaContext context);

  /// <summary>Represents an implementation of a lambda function.</summary>
  public delegate Task<object> LambdaHandler(object input, ILambdaContext context, CancellationToken cancellationToken = default);

  /// <summary>An emulator for Amazon's Lambda Service.</summary>
  public sealed class AmazonLambdaEmulator : IAmazonServiceEmulator<IAmazonLambda>
  {
    private readonly LambdaResolver resolver;

    public AmazonLambdaEmulator(LambdaResolver resolver)
    {
      Check.NotNull(resolver, nameof(resolver));

      this.resolver = resolver;

      Client = new EmulatedAmazonLambda(this);
    }

    public IAmazonLambda Client { get; }

    /// <summary>Resolves a <see cref="LambdaHandler"/> for the given context.</summary>
    internal LambdaHandler ResolveHandler(object input, ILambdaContext context)
    {
      Check.NotNull(context, nameof(context));

      return resolver(input, context);
    }

    /// <summary>Resolves and executes a lambda for the given input and context.</summary>
    internal async Task<object> ExecuteLambdaAsync(object input, ILambdaContext context, CancellationToken cancellationToken = default)
    {
      var handler = ResolveHandler(input, context);

      using (var timeoutSource = new CancellationTokenSource(context.RemainingTime))
      using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(timeoutSource.Token, cancellationToken))
      {
        return await handler(input, context, linkedSource.Token);
      }
    }
  }
}