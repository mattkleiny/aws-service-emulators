using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.StepFunction;
using Amazon.StepFunctions.Internal;
using Amazon.StepFunctions.Model;

namespace Amazon.StepFunctions
{
  /// <summary>Resolves the specification JSON for the given state machine ARN.</summary>
  public delegate string SpecificationResolver(StateMachineARN arn);

  /// <summary>An emulator for Amazon's StepFunctions.</summary>
  public sealed class AmazonStepFunctionsEmulator : IAmazonServiceEmulator<IAmazonStepFunctions>
  {
    private readonly ConcurrentDictionary<string, StepFunctionHost> hostsByArn = new ConcurrentDictionary<string, StepFunctionHost>();
    private readonly ConcurrentBag<Execution>                       executions = new ConcurrentBag<Execution>();

    private readonly SpecificationResolver resolver;
    private readonly StepHandlerFactory    factory;
    private readonly Impositions           impositions;

    public AmazonStepFunctionsEmulator(SpecificationResolver resolver, StepHandlerFactory factory)
      : this(resolver, factory, Impositions.Default)
    {
    }

    public AmazonStepFunctionsEmulator(SpecificationResolver resolver, StepHandlerFactory factory, Impositions impositions)
    {
      Check.NotNull(resolver, nameof(resolver));
      Check.NotNull(factory, nameof(factory));
      Check.NotNull(impositions, nameof(impositions));

      this.resolver    = resolver;
      this.factory     = factory;
      this.impositions = impositions;

      Client = new DelegatingAmazonStepFunctions(this);
    }

    public IAmazonStepFunctions Client { get; }

    /// <summary>A list of the <see cref="Execution"/>s that have occurred in this emulator.</summary>
    public IEnumerable<Execution> Executions => executions;

    /// <summary>Retrieves or creates a new <see cref="StepFunctionHost"/> for the given <see cref="arn"/>.</summary>
    public StepFunctionHost GetOrCreateHost(string arn)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var stateMachineARN = StateMachineARN.Parse(arn);

      return hostsByArn.GetOrAdd(arn, _ => StepFunctionHost.FromJson(resolver(stateMachineARN), factory));
    }

    /// <summary>Schedules the execution of a <see cref="StepFunctionHost"/> with the given input and retains it's execution information.</summary>
    public void ScheduleExecution(string arn, object input)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var task = Task.Factory.StartNew(
        () => ExecuteAsync(arn, input).Result,
        TaskCreationOptions.LongRunning
      );

      executions.Add(new Execution(task));
    }

    /// <summary>Executes a <see cref="StepFunctionHost"/> with the given input.</summary>
    public Task<StepFunctionHost.Result> ExecuteAsync(string arn, object input, CancellationToken cancellationToken = default)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var host = GetOrCreateHost(arn);

      return host.ExecuteAsync(impositions, input, cancellationToken);
    }

    /// <summary>Encapsulates an execution that has occurred in this emulator.</summary>
    public sealed class Execution
    {
      public Execution(Task<StepFunctionHost.Result> task)
      {
        Check.NotNull(task, nameof(task));

        State = ExecutionState.Processing;

        task.ContinueWith(parent =>
        {
          if (parent.IsFaulted)
          {
            State     = ExecutionState.Failed;
            Exception = parent.Exception;
          }
          else
          {
            Result = parent.Result;

            if (Result.IsFailure)
            {
              State     = ExecutionState.Failed;
              Exception = parent.Result.Exception;
            }
            else
            {
              State = ExecutionState.Completed;
            }
          }

          EndTime = DateTime.Now;
        });
      }

      public Guid           Id        { get; } = Guid.NewGuid();
      public DateTime       StartTime { get; } = DateTime.Now;
      public DateTime?      EndTime   { get; private set; }
      public ExecutionState State     { get; private set; }
      public Exception      Exception { get; private set; }

      public StepFunctionHost.Result Result { get; private set; }

      public override string ToString() => $"{Id} - {State} (started at {StartTime})";
    }

    /// <summary>Encapsulates the possibles states for an <see cref="Execution"/>.</summary>
    public enum ExecutionState
    {
      Processing,
      Completed,
      Failed
    }
  }
}