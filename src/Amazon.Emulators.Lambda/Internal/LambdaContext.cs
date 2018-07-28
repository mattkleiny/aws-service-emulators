using System;
using Amazon.Lambda.Core;

namespace Amazon.Lambda.Internal
{
  /// <summary>A <see cref="ILambdaContext"/> for emulated execution.</summary>
  internal sealed class LambdaContext : ILambdaContext
  {
    public LambdaContext(string functionName)
      : this(functionName, "$LATEST")
    {
    }

    public LambdaContext(string functionName, string qualifier)
    {
      Check.NotNullOrEmpty(functionName, nameof(functionName));

      FunctionName    = functionName;
      FunctionVersion = qualifier ?? "$LATEST";
    }

    public string           AwsRequestId       { get; } = Guid.NewGuid().ToString();
    public IClientContext   ClientContext      { get; } = null;
    public string           FunctionName       { get; }
    public string           FunctionVersion    { get; }
    public ICognitoIdentity Identity           { get; } = null;
    public string           InvokedFunctionArn { get; } = string.Empty;
    public ILambdaLogger    Logger             { get; } = null;
    public string           LogGroupName       { get; } = string.Empty;
    public string           LogStreamName      { get; } = string.Empty;
    public int              MemoryLimitInMB    { get; } = 3000;                      // maximum mb in aws
    public TimeSpan         RemainingTime      { get; } = TimeSpan.FromSeconds(300); // maximum time in aws
  }
}