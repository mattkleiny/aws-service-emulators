﻿using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Internal;
using Xunit;

namespace Amazon.Emulators.Lambda.Tests
{
  public class AmazonLambdaEmulatorTests
  {
    private readonly AmazonLambdaEmulator emulator = new(resolver: _ => TestHandler);

    [Fact]
    public async Task it_should_execute_resolved_lambdas()
    {
      var context = new LambdaContext("test-handler");
      var output  = await emulator.ExecuteLambdaAsync("test", context);

      Assert.Equal("TEST", output);
    }

    private static Task<object> TestHandler(object input, ILambdaContext context, CancellationToken cancellationtoken)
    {
      return Task.FromResult<object>(input.ToString().ToUpper());
    }
  }
}