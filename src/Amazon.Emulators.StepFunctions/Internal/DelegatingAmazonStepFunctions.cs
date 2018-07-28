using System;
using System.Linq;
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

      if (!emulator.StateMachines.TryGetValue(stateMachineArn, out var machine))
      {
        return Task.FromResult(new ListExecutionsResponse
        {
          HttpStatusCode = HttpStatusCode.NotFound
        });
      }

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