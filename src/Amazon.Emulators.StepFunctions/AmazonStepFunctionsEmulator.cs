using System;
using System.Collections.Concurrent;
using Amazon.Emulators;
using Amazon.StepFunction;
using Amazon.StepFunction.Hosting;
using Amazon.StepFunctions.Internal;
using Amazon.StepFunctions.Model;

namespace Amazon.StepFunctions
{
  /// <summary>Resolves the specification JSON for the given state machine.</summary>
  public delegate string SpecificationResolver(RegionEndpoint region, long accountId, string stateMachineName);

  /// <summary>An emulator for Amazon's StepFunctions.</summary>
  public sealed class AmazonStepFunctionsEmulator : IAmazonServiceEmulator<IAmazonStepFunctions>
  {
    private readonly ConcurrentDictionary<string, StateMachine> machinesByArn = new ConcurrentDictionary<string, StateMachine>(StringComparer.OrdinalIgnoreCase);

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

      Client = new EmulatedAmazonStepFunctions(this);
    }

    public IAmazonStepFunctions Client { get; }

    /// <summary>Retrieves the raw specification for the given <see cref="StateMachineARN"/>.</summary>
    internal string GetSpecification(string arn)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var parsed = StateMachineARN.Parse(arn);

      return resolver(parsed.Region, parsed.AccountId, parsed.StateMachineName);
    }

    /// <summary>Retrieves or creates the <see cref="StepFunctionHost"/> for the given <see cref="StateMachineARN"/>.</summary>
    internal StateMachine GetOrCreateStateMachine(string arn)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var parsed = StateMachineARN.Parse(arn);

      return machinesByArn.GetOrAdd(arn, _ =>
      {
        var specification = resolver(parsed.Region, parsed.AccountId, parsed.StateMachineName);
        var host          = StepFunctionHost.FromJson(specification, factory);

        return new StateMachine(parsed, host, impositions);
      });
    }

    /// <summary>Schedules the execution of a <see cref="StepFunctionHost"/> with the given input and retains it's <see cref="Execution"/> information.</summary>
    internal ExecutionARN ScheduleExecution(string arn, string executionName, object input)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));
      Check.NotNullOrEmpty(executionName, nameof(executionName));

      var machine = GetOrCreateStateMachine(arn);

      return machine.StartExecution(executionName, input);
    }
  }
}