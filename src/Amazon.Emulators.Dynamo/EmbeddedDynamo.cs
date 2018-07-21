using Amazon.DynamoDBv2;
using Amazon.Emulators.Embedded;

namespace Amazon.Dynamo
{
  /// <summary>An embedded implementation of <see cref="IAmazonDynamoDB"/>.</summary>
  public sealed class EmbeddedDynamo : EmbeddedService<IAmazonDynamoDB>
  {
    public override IAmazonDynamoDB Client { get; }
  }
}