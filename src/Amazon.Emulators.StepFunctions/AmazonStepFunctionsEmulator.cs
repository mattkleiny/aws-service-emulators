using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.StepFunction;
using Amazon.StepFunctions.Internal;

namespace Amazon.StepFunctions
{
  /// <summary>Resolves the specification JSON for the given state machine ARN.</summary>
  public delegate string SpecificationResolver(string arn);

  /// <summary>An emulator for Amazon's StepFunctions.</summary>
  public sealed class AmazonStepFunctionsEmulator : IAmazonServiceEmulator<IAmazonStepFunctions>
  {
    private readonly ConcurrentDictionary<string, StepFunctionHost> machines = new ConcurrentDictionary<string, StepFunctionHost>();

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

    /// <summary>Retrieves or creates a new <see cref="StepFunctionHost"/> for the given <see cref="arn"/>.</summary>
    public StepFunctionHost GetOrCreateHost(string arn)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      return machines.GetOrAdd(arn, _ => StepFunctionHost.FromJson(resolver(arn), factory));
    }

    /// <summary>Executes a <see cref="StepFunctionHost"/> with the given input.</summary>
    public Task<StepFunctionHost.Result> ExecuteMachineAsync(string arn, object input, CancellationToken cancellationToken = default)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var host = GetOrCreateHost(arn);

      return host.ExecuteAsync(impositions, input, cancellationToken);
    }
  }
}