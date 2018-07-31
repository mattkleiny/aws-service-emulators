using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Xunit;

namespace Amazon.Emulators.StepFunctions.Tests.Internal
{
  public class EmulatedAmazonStepFunctions
  {
    private const string TestMachineARN = "arn:aws:states:ap-southeast-2:123456789:stateMachine:test-machine";

    public EmulatorFixture Fixture { get; } = new EmulatorFixture();

    public AmazonStepFunctionsEmulator Emulator => Fixture.Emulator;
    public IAmazonStepFunctions        Client   => Emulator.Client;

    [Fact]
    public async Task it_should_DescribeStateMachines_without_fault()
    {
      var response = await Client.DescribeStateMachineAsync(new DescribeStateMachineRequest
      {
        StateMachineArn = TestMachineARN
      });

      Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
      Assert.NotEmpty(response.StateMachineArn);
      Assert.NotEmpty(response.Name);
      Assert.NotEmpty(response.Definition);
    }

    [Fact]
    public async void it_should_StartExecution_without_fault()
    {
      var response = await Client.StartExecutionAsync(new StartExecutionRequest
      {
        Name            = Guid.NewGuid().ToString(),
        StateMachineArn = TestMachineARN,
        Input           = "Hello, World!"
      });

      Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
      Assert.NotEmpty(response.ExecutionArn);
    }

    [Fact]
    public async Task it_should_ListExecutions_without_fault()
    {
      var response = await Client.ListExecutionsAsync(new ListExecutionsRequest
      {
        StateMachineArn = TestMachineARN,
        MaxResults      = 10
      });

      Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
      Assert.NotNull(response.Executions);
    }
  }
}