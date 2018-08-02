using System;
using System.Threading.Tasks;
using Amazon.StepFunctions;

namespace Amazon.Emulators.StepFunctions.Tests
{
  /// <summary>A test fixture for the <see cref="AmazonStepFunctionsEmulator"/>.</summary>
  public sealed class EmulatorFixture
  {
    public AmazonStepFunctionsEmulator Emulator { get; } = new AmazonStepFunctionsEmulator(
      resolver: (region, id, name) =>
      {
        switch (name.ToLower())
        {
          case "test-machine":
            return EmbeddedResources.TestMachine;

          default:
            throw new Exception($"An unrecognized state machine was requested: {name}");
        }
      },
      factory: definition => ((input, cancellationtoken) => Task.FromResult(input))
    );
  }
}