using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Model;
using Newtonsoft.Json;

namespace Amazon.Lambda.Internal
{
  // TODO: support serialization to the right lambda parameter type

  /// <summary>An <see cref="IAmazonLambda"/> implementation that delegates directly to an <see cref="AmazonLambdaEmulator"/>.</summary>
  internal sealed class EmulatedAmazonLambda : AmazonLambdaBase
  {
    private readonly AmazonLambdaEmulator emulator;

    public EmulatedAmazonLambda(AmazonLambdaEmulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }

    public override async Task<InvokeResponse> InvokeAsync(InvokeRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));
 
      var context = new LambdaContext(request.FunctionName, request.Qualifier);

      // test invocation
      if (request.InvocationType == InvocationType.DryRun)
      {
        var handler = emulator.ResolveHandler(context);

        if (handler == null)
        {
          return new InvokeResponse
          {
            HttpStatusCode = HttpStatusCode.NotFound
          };
        }

        return new InvokeResponse
        {
          HttpStatusCode = HttpStatusCode.OK
        };
      }

      // asynchronous invocation
      if (request.InvocationType == InvocationType.Event)
      {
        emulator.ScheduleLambda(request.Payload, context);

        return new InvokeResponse
        {
          HttpStatusCode = HttpStatusCode.Accepted
        };
      }

      // synchronous invocation / default handler
      if (request.InvocationType == InvocationType.RequestResponse || request.InvocationType == null)
      {
        var output = await emulator.ExecuteLambdaAsync(request.Payload, context, cancellationToken);
        var json   = JsonConvert.SerializeObject(output);

        return new InvokeResponse
        {
          ExecutedVersion = context.FunctionVersion,
          Payload         = new MemoryStream(Encoding.UTF8.GetBytes(json)),
          HttpStatusCode  = HttpStatusCode.OK
        };
      }

      throw new InvalidOperationException($"An unrecognized invocation type was requested: {request.InvocationType}");
    }
  }
}