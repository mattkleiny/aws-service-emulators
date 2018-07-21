using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;

namespace Amazon.Lambda
{
  // TODO: give this a massive once-over
  
  internal sealed class EmbeddedLambdaClient : EmbeddedLambdaClientBase
  {
    private readonly EmbeddedAmazonLambda parent;

    public EmbeddedLambdaClient(EmbeddedAmazonLambda parent)
    {
      Check.NotNull(parent, nameof(parent));

      this.parent = parent;
    }

    public override async Task<InvokeResponse> InvokeAsync(InvokeRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var context = new EmbeddedLambdaContext(request.FunctionName, request.Qualifier);
      var handler = parent.ResolveLambdaHandler(request.Payload, context);
      var result  = await handler(request.Payload, context, cancellationToken);

      return new InvokeResponse
      {
        ExecutedVersion = request.Qualifier,
        Payload         = new MemoryStream(Encoding.UTF8.GetBytes(result.ToString())),
        HttpStatusCode  = HttpStatusCode.OK
      };
    }

    public override Task<InvokeAsyncResponse> InvokeAsyncAsync(InvokeAsyncRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var context = new EmbeddedLambdaContext(request.FunctionName, "$LATEST");
      var handler = parent.ResolveLambdaHandler(request.FunctionName, context);

      Task.Run(async () => await handler(request.InvokeArgs, context, cancellationToken), cancellationToken);

      return Task.FromResult(new InvokeAsyncResponse
      {
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    /// <summary>A <see cref="Amazon.Lambda.Core.ILambdaContext"/> for embedded invocation.</summary>
    private sealed class EmbeddedLambdaContext : ILambdaContext
    {
      public EmbeddedLambdaContext(string functionName, string functionVersion)
      {
        Check.NotNullOrEmpty(functionName, nameof(functionName));

        FunctionName    = functionName;
        FunctionVersion = functionVersion;
      }

      public string           AwsRequestId       { get; } = Guid.NewGuid().ToString();
      public IClientContext   ClientContext      { get; } = null;
      public string           FunctionName       { get; }
      public string           FunctionVersion    { get; }
      public ICognitoIdentity Identity           { get; } = null;
      public string           InvokedFunctionArn { get; } = null;
      public ILambdaLogger    Logger             { get; } = null;
      public string           LogGroupName       { get; } = string.Empty;
      public string           LogStreamName      { get; } = string.Empty;
      public int              MemoryLimitInMB    { get; } = int.MaxValue;
      public TimeSpan         RemainingTime      { get; } = TimeSpan.MaxValue;
    }
  }
}