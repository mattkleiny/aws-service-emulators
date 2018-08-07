using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.StepFunction.Model;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;

namespace Amazon.StepFunction.Internal
{
  /// <summary>An <see cref="IAmazonStepFunctions"/> implementation that delegates directly to an <see cref="AmazonStepFunctionsEmulator"/>.</summary>
  internal sealed class StepFunctionsEmulatorClient : AmazonStepFunctionsBase
  {
    private readonly AmazonStepFunctionsEmulator emulator;

    public StepFunctionsEmulatorClient(AmazonStepFunctionsEmulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }

    public override Task<DescribeStateMachineResponse> DescribeStateMachineAsync(DescribeStateMachineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      Check.NotNull(request, nameof(request));

      var machine = emulator.GetOrCreateStateMachine(request.StateMachineArn);

      return Task.FromResult(new DescribeStateMachineResponse
      {
        StateMachineArn = machine.ARN.ToString(),
        Name            = machine.ARN.StateMachineName,
        Definition      = emulator.GetSpecification(request.StateMachineArn),
        Status          = StateMachineStatus.ACTIVE,
        HttpStatusCode  = HttpStatusCode.OK
      });
    }

    public override Task<StartExecutionResponse> StartExecutionAsync(StartExecutionRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var executionArn = emulator.ScheduleExecution(request.StateMachineArn, request.Name, request.Input);

      return Task.FromResult(new StartExecutionResponse
      {
        ExecutionArn   = executionArn.ToString(),
        StartDate      = DateTime.Now,
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override Task<ListExecutionsResponse> ListExecutionsAsync(ListExecutionsRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      ExecutionStatus Map(ExecutionState state)
      {
        switch (state)
        {
          case ExecutionState.Completed:
            return ExecutionStatus.SUCCEEDED;

          case ExecutionState.Failed:
            return ExecutionStatus.FAILED;

          default:
            return ExecutionStatus.RUNNING;
        }
      }

      // TODO: what about the pagination?
      // TODO: what about the filters?

      var machine = emulator.GetOrCreateStateMachine(request.StateMachineArn);

      return Task.FromResult(new ListExecutionsResponse
      {
        Executions = machine.Executions.Values
          .Take(request.MaxResults)
          .Select(execution => new ExecutionListItem
          {
            StateMachineArn = machine.ARN.ToString(),
            ExecutionArn    = execution.ARN.ToString(),
            Name            = execution.ARN.ExecutionName,
            StartDate       = execution.StartDate,
            StopDate        = execution.StopDate.GetValueOrDefault(DateTime.MaxValue),
            Status          = Map(execution.Status)
          })
          .ToList(),
        HttpStatusCode = HttpStatusCode.OK
      });
    }
  }
}