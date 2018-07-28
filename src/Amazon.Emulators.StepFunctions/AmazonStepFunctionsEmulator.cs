using System.Collections.Concurrent;
using System.Collections.Generic;
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
    private readonly ConcurrentDictionary<StateMachineARN, StateMachine> stateMachinesByArn = new ConcurrentDictionary<StateMachineARN, StateMachine>();

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

    /// <summary>The <see cref="StateMachine"/>s managed by this emulator. </summary>
    internal IReadOnlyDictionary<StateMachineARN, StateMachine> StateMachines => stateMachinesByArn;

    /// <summary>Retrieves or creates the <see cref="StepFunctionHost"/> for the given <see cref="StateMachineARN"/>.</summary>
    internal StateMachine GetOrCreateStateMachine(StateMachineARN arn)
    {
      Check.NotNull(arn, nameof(arn));

      return stateMachinesByArn.GetOrAdd(arn, _ =>
      {
        var specification = resolver(_);
        var host          = StepFunctionHost.FromJson(specification, factory);

        return new StateMachine(_, host, impositions);
      });
    }

    /// <summary>Schedules the execution of a <see cref="StepFunctionHost"/> with the given input and retains it's <see cref="Execution"/> information.</summary>
    internal ExecutionARN ScheduleExecution(StateMachineARN stateMachineArn, string executionName, object input)
    {
      Check.NotNull(stateMachineArn, nameof(stateMachineArn));
      Check.NotNullOrEmpty(executionName, nameof(executionName));

      var machine = GetOrCreateStateMachine(stateMachineArn);

      return machine.StartExecution(executionName, input);
    }
  }
}