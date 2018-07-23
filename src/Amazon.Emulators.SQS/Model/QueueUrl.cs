using System;
using System.Text.RegularExpressions;

namespace Amazon.SQS.Model
{
  /// <summary>Models the URL for an Amazon SQS <see cref="IQueue"/>.</summary>
  public sealed class QueueUrl
  {
    private static readonly Regex Regex = new Regex(@"^https?:\/\/sqs\.([a-zA-Z0-9\-]+)\.amazonaws\.com\/([0-9]+)\/([a-zA-Z0-9\-]+)\/?");

    /// <summary>Attemps to parse a <see cref="QueueUrl"/> from the given raw url.</summary>
    public static QueueUrl Parse(string url)
    {
      Check.NotNullOrEmpty(url, nameof(url));

      var match = Regex.Match(url);

      if (!match.Success)
      {
        throw new InvalidQueueUrlException(url);
      }

      var region    = RegionEndpoint.GetBySystemName(match.Groups[1].Value);
      var accountId = long.Parse(match.Groups[2].Value);
      var name      = match.Groups[3].Value;

      return new QueueUrl(region, accountId, name);
    }

    public QueueUrl(RegionEndpoint endpoint, long accountId, string name)
    {
      Endpoint  = endpoint;
      AccountId = accountId;
      Name      = name;
    }

    public RegionEndpoint Endpoint  { get; }
    public long           AccountId { get; }
    public string         Name      { get; }

    public override string ToString()
    {
      return string.Intern($"https://sqs.{Endpoint.SystemName}.amazonaws.com/{AccountId}/{Name}");
    }

    private bool Equals(QueueUrl other)
    {
      return AccountId == other.AccountId && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      return obj is QueueUrl url && Equals(url);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = Endpoint.GetHashCode();

        hashCode = (hashCode * 397) ^ AccountId.GetHashCode();
        hashCode = (hashCode * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Name);

        return hashCode;
      }
    }
  }
}