using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.StepFunctions.Model;

namespace Amazon.StepFunctions.Internal
{
  /// <summary>An <see cref="IAmazonStepFunctions"/> implementation that delegates directly to an <see cref="AmazonStepFunctionsEmulator"/>.</summary>
  internal sealed class EmulatedAmazonStepFunctions : AmazonStepFunctionsBase
  {
    private readonly AmazonStepFunctionsEmulator emulator;

    public EmulatedAmazonStepFunctions(AmazonStepFunctionsEmulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }

    public override Task<DescribeStateMachineResponse> DescribeStateMachineAsync(DescribeStateMachineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      Check.NotNull(request, nameof(request));

      var stateMachineARN = StateMachineARN.Parse(request.StateMachineArn);
      var machine         = emulator.GetOrCreateStateMachine(stateMachineARN);

      return Task.FromResult(new DescribeStateMachineResponse
      {
        StateMachineArn = machine.ARN.ToString(),
        Definition      = emulator.GetSpecification(stateMachineARN),
        Name            = stateMachineARN.StateMachineName,
        Status          = StateMachineStatus.ACTIVE,
        HttpStatusCode  = HttpStatusCode.OK
      });
    }

    public override Task<StartExecutionResponse> StartExecutionAsync(StartExecutionRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var stateMachineArn = StateMachineARN.Parse(request.StateMachineArn);
      var executionArn    = emulator.ScheduleExecution(stateMachineArn, request.Name, request.Input);

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

      var stateMachineArn = StateMachineARN.Parse(request.StateMachineArn);
      var machine         = emulator.GetOrCreateStateMachine(stateMachineArn);

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