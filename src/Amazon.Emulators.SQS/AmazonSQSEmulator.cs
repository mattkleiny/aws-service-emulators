using System.Collections.Concurrent;
using Amazon.Emulators;
using Amazon.SQS.Internal;
using Amazon.SQS.Model;

namespace Amazon.SQS
{
  /// <summary>A factory for <see cref="IQueue"/>s based on their <see cref="QueueUrl"/>.</summary>
  public delegate IQueue QueueFactory(QueueUrl url);

  /// <summary>An emulator for Amazon's Simple Queue Service (SQS).</summary>
  public sealed class AmazonSQSEmulator : IAmazonServiceEmulator<IAmazonSQS>
  {
    private readonly ConcurrentDictionary<QueueUrl, IQueue> queuesByUrl = new();

    private readonly RegionEndpoint endpoint;
    private readonly long           accountId;
    private readonly QueueFactory   factory;

    public AmazonSQSEmulator(RegionEndpoint endpoint, long accountId, QueueFactory factory)
    {
      Check.NotNull(endpoint, nameof(endpoint));
      Check.That(accountId > 0, "accountId > 0");
      Check.NotNull(factory, nameof(factory));

      this.endpoint  = endpoint;
      this.accountId = accountId;
      this.factory   = factory;

      Client = new EmulatedAmazonSQS(this);
    }

    public IAmazonSQS Client { get; }

    /// <summary>Retrieves an existing <see cref="IQueue"/>, or creates a new one, given its <see cref="name"/>.</summary>
    internal IQueue GetOrCreateByName(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      var queueUrl = new QueueUrl(endpoint, accountId, name);

      return queuesByUrl.GetOrAdd(queueUrl, _ => factory(_));
    }

    /// <summary>Retrieves an existing <see cref="IQueue"/>, or creates a new one, given its <see cref="url"/>.</summary>
    internal IQueue GetOrCreateByUrl(string url)
    {
      Check.NotNullOrEmpty(url, nameof(url));

      var queueUrl = QueueUrl.Parse(url);

      return queuesByUrl.GetOrAdd(queueUrl, _ => factory(_));
    }
  }
}