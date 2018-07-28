using System;
using System.Text.RegularExpressions;

namespace Amazon.StepFunctions.Model
{
  /// <summary>Encapsulates an ARN for an AWS state machine execution.</summary>
  internal sealed class ExecutionARN
  {
    private static readonly Regex Regex = new Regex(@"^arn:aws:states:([a-zA-Z0-9\-]+):([0-9]+):execution:([a-zA-Z0-9\-]+):([a-zA-Z0-9\-_]+)$");

    /// <summary>Parses a <see cref="ExecutionARN"/> from the given string.</summary>
    public static ExecutionARN Parse(string arn)
    {
      Check.NotNullOrEmpty(arn, nameof(arn));

      var match = Regex.Match(arn);
      if (!match.Success)
      {
        throw new InvalidARNException(arn);
      }

      var region           = RegionEndpoint.GetBySystemName(match.Groups[1].Value);
      var accountId        = long.Parse(match.Groups[2].Value);
      var stateMachineName = match.Groups[3].Value;
      var executionName    = match.Groups[4].Value;

      return new ExecutionARN(region, accountId, stateMachineName, executionName);
    }

    public ExecutionARN(RegionEndpoint region, long accountId, string stateMachineName, string executionName)
    {
      Check.NotNull(region, nameof(region));
      Check.That(accountId > 0, "accountId > 0");
      Check.NotNullOrEmpty(stateMachineName, nameof(stateMachineName));
      Check.NotNullOrEmpty(executionName, nameof(executionName));

      Region           = region;
      AccountId        = accountId;
      StateMachineName = stateMachineName;
      ExecutionName    = executionName;
    }

    public RegionEndpoint Region           { get; }
    public long           AccountId        { get; }
    public string         StateMachineName { get; }
    public string         ExecutionName    { get; }

    public override string ToString() => $"arn:aws:states:{Region.SystemName}:{AccountId}:execution:{StateMachineName}:{ExecutionName}";

    private bool Equals(ExecutionARN other)
    {
      return Equals(Region, other.Region)                                                                &&
             AccountId == other.AccountId                                                                &&
             string.Equals(StateMachineName, other.StateMachineName, StringComparison.OrdinalIgnoreCase) &&
             string.Equals(ExecutionName, other.ExecutionName, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      return obj is ExecutionARN arn && Equals(arn);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = Region.GetHashCode();

        hashCode = (hashCode * 397) ^ AccountId.GetHashCode();
        hashCode = (hashCode * 397) ^ StateMachineName.GetHashCode();
        hashCode = (hashCode * 397) ^ ExecutionName.GetHashCode();

        return hashCode;
      }
    }
  }
}