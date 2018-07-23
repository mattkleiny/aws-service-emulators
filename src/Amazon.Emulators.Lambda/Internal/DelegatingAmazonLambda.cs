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
  // TODO: support invocation types

  /// <summary>An <see cref="IAmazonLambda"/> implementation that delegates directly to an <see cref="AmazonLambdaEmulator"/>.</summary>
  internal sealed class DelegatingAmazonLambda : AmazonLambdaBase
  {
    private readonly AmazonLambdaEmulator emulator;

    public DelegatingAmazonLambda(AmazonLambdaEmulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }

    public override async Task<InvokeResponse> InvokeAsync(InvokeRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var context = new EmulatedLambdaContext(request.FunctionName);

      if (request.InvocationType == InvocationType.DryRun)
      {
        throw new NotImplementedException();
      }

      if (request.InvocationType == InvocationType.Event)
      {
        throw new NotImplementedException();
      }

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