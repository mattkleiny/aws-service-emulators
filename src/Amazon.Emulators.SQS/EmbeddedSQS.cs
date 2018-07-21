using System.Collections.Concurrent;
using Amazon.Emulators.Embedded;
using Amazon.SQS.Model;

namespace Amazon.SQS
{
  /// <summary>An embedded implementation of <see cref="IAmazonSQS"/>.</summary>
  public sealed class EmbeddedSQS : EmbeddedService<IAmazonSQS>
  {
    private readonly ConcurrentDictionary<string, Queue> queuesByName = new ConcurrentDictionary<string, Queue>();
    private readonly ConcurrentDictionary<string, Queue> queuesByUrl  = new ConcurrentDictionary<string, Queue>();

    public EmbeddedSQS() => Client = new EmbeddedSQSClient(this);

    public override IAmazonSQS Client { get; }

    internal bool TryGetQueueByName(string name, out Queue queue)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      return queuesByName.TryGetValue(name, out queue);
    }

    internal bool TryGetQueueByUrl(string url, out Queue queue)
    {
      Check.NotNullOrEmpty(url, nameof(url));

      return queuesByUrl.TryGetValue(url, out queue);
    }
  }
}