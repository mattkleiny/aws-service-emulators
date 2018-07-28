using System;
using System.Threading.Tasks;
using Amazon.StepFunctions;

namespace Amazon.Emulators.StepFunctions.Tests
{
  /// <summary>A test fixture for the <see cref="AmazonStepFunctionsEmulator"/>.</summary>
  public sealed class AmazonStepFunctionsEmulatorFixture
  {
    public AmazonStepFunctionsEmulator Emulator { get; } = new AmazonStepFunctionsEmulator(
      resolver: arn =>
      {
        switch (arn.StateMachineName.ToLower())
        {
          case "test-machine":
            return EmbeddedResources.TestMachine;

          default:
            throw new Exception($"An unrecognized state machine was requested: {arn}");
        }
      },
      factory: definition => ((input, cancellationtoken) => Task.FromResult(input))
    );
  }
}