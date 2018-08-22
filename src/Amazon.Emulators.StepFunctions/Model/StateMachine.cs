using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.StepFunction.Hosting;

namespace Amazon.StepFunctions.Model
{
  /// <summary>Models a state machine and records it's <see cref="Execution"/>s.</summary>
  internal sealed class StateMachine
  {
    private readonly ConcurrentDictionary<string, Execution> executionsByArn = new ConcurrentDictionary<string, Execution>(StringComparer.OrdinalIgnoreCase);

    private readonly StepFunctionHost host;
    private readonly Impositions      impositions;

    public StateMachine(StateMachineARN arn, StepFunctionHost host, Impositions impositions)
    {
      Check.NotNull(arn, nameof(arn));
      Check.NotNull(host, nameof(host));
      Check.NotNull(impositions, nameof(impositions));

      this.host        = host;
      this.impositions = impositions;

      ARN = arn;
    }

    /// <summary>The <see cref="StateMachineARN"/> of this particular machine.</summary>
    public StateMachineARN ARN { get; }

    /// <summary>The <see cref="Execution"/>s that have occurred in this emulator.</summary>
    public IReadOnlyDictionary<string, Execution> Executions => executionsByArn;

    /// <summary>Starts the execution of the state machine with the given name and input.</summary>
    public ExecutionARN StartExecution(string executionName, object input)
    {
      Check.NotNullOrEmpty(executionName, nameof(executionName));

      var executionArn = new ExecutionARN(
        ARN.Region,
        ARN.AccountId,
        ARN.StateMachineName,
        executionName
      );

      executionsByArn.AddOrUpdate(executionArn.ToString(),
        addValueFactory: _ =>
        {
          var task = Task.Factory.StartNew(
            () => host.ExecuteAsync(impositions, input).Result,
            TaskCreationOptions.LongRunning
          );

          return new Execution(task, executionArn);
        },
        updateValueFactory: (name, existing) => throw new InvalidOperationException($"The given execution name '{executionName}' already exists.")
      );

      return executionArn;
    }
  }
}