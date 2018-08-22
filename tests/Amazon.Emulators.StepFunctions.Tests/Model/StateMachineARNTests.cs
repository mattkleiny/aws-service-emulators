using Amazon.StepFunctions.Model;
using Xunit;

namespace Amazon.Emulators.StepFunctions.Tests.Model
{
  public class StateMachineARNTests
  {
    [Fact]
    public void it_should_produce_a_valid_ARN_from_components()
    {
      var arn = new StateMachineARN(RegionEndpoint.APSoutheast2, 123456789, "test-machine");

      Assert.Equal("arn:aws:states:ap-southeast-2:123456789:stateMachine:test-machine", arn.ToString());
    }

    [Theory]
    [InlineData("arn:aws:states:ap-southeast-2:123456789:stateMachine:test-machine")]
    public void it_should_be_able_to_parse_simple_arns(string arn) => Assert.NotNull(StateMachineARN.Parse(arn));
  }
}