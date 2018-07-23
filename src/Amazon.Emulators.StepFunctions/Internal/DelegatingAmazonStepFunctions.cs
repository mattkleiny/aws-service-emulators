﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.StepFunctions.Model;

namespace Amazon.StepFunctions.Internal
{
  /// <summary>An <see cref="IAmazonStepFunctions"/> implementation that delegates directly to an <see cref="AmazonStepFunctionsEmulator"/>.</summary>
  internal sealed class DelegatingAmazonStepFunctions : AmazonStepFunctionsBase
  {
    private readonly AmazonStepFunctionsEmulator emulator;

    public DelegatingAmazonStepFunctions(AmazonStepFunctionsEmulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }

    public override Task<StartExecutionResponse> StartExecutionAsync(StartExecutionRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      emulator.ScheduleExecution(request.StateMachineArn, request.Input);

      return Task.FromResult(new StartExecutionResponse
      {
        StartDate      = DateTime.Now,
        HttpStatusCode = HttpStatusCode.OK
      });
    }
  }
}