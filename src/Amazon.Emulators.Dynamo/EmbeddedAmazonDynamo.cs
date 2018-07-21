using Amazon.DynamoDBv2;
using Amazon.Emulators.Embedded;

namespace Amazon.Dynamo
{
  /// <summary>An embedded implementation of <see cref="IAmazonDynamoDB"/>.</summary>
  public sealed class EmbeddedAmazonDynamo : IEmbeddedAmazonService<IAmazonDynamoDB>
  {
    public IAmazonDynamoDB Client { get; }
  }
}