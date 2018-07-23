using System;
using Amazon.Lambda.Core;

namespace Amazon.Lambda.Model
{
  /// <summary>A <see cref="ILambdaContext"/> for emulated execution.</summary>
  internal sealed class EmulatedLambdaContext : ILambdaContext
  {
    public EmulatedLambdaContext(string functionName)
    {
      Check.NotNullOrEmpty(functionName, nameof(functionName));

      FunctionName = functionName;
    }

    public string           AwsRequestId       { get; } = Guid.NewGuid().ToString();
    public IClientContext   ClientContext      { get; } = null;
    public string           FunctionName       { get; }
    public string           FunctionVersion    { get; } = "$LATEST";
    public ICognitoIdentity Identity           { get; } = null;
    public string           InvokedFunctionArn { get; } = string.Empty;
    public ILambdaLogger    Logger             { get; } = null;
    public string           LogGroupName       { get; } = string.Empty;
    public string           LogStreamName      { get; } = string.Empty;
    public int              MemoryLimitInMB    { get; } = int.MaxValue;
    public TimeSpan         RemainingTime      { get; } = TimeSpan.MaxValue;
  }
}