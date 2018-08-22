using Amazon.StepFunctions.Model;
using Xunit;

namespace Amazon.Emulators.StepFunctions.Tests.Model
{
  public class ExecutionARNTests
  {
    [Fact]
    public void it_should_produce_a_valid_ARN_from_components()
    {
      var arn = new ExecutionARN(RegionEndpoint.APSoutheast2, 123456789, "test-machine", "123456_test_execution");

      Assert.Equal("arn:aws:states:ap-southeast-2:123456789:execution:test-machine:123456_test_execution", arn.ToString());
    }

    [Theory]
    [InlineData("arn:aws:states:ap-southeast-2:123456789:execution:test-machine:123456_test_execution")]
    public void it_should_be_able_to_parse_simple_arns(string arn) => Assert.NotNull(ExecutionARN.Parse(arn));
  }
}