using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Model;
using Newtonsoft.Json;

namespace Amazon.Lambda.Internal
{
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

      // TODO: support serialization to the right lambda parameter type

      var context = new EmulatedLambdaContext(request.FunctionName);
      var output  = await emulator.ExecuteLambdaAsync(request.Payload, context, cancellationToken);

      var json = JsonConvert.SerializeObject(output);

      return new InvokeResponse
      {
        Payload         = new MemoryStream(Encoding.UTF8.GetBytes(json)),
        ExecutedVersion = context.FunctionVersion,
        HttpStatusCode  = HttpStatusCode.OK
      };
    }
  }
}