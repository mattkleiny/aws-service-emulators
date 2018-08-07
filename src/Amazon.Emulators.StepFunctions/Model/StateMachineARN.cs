using System;
using System.Text.RegularExpressions;

namespace Amazon.StepFunction.Model
{
  /// <summary>Encapsulates an ARN for an AWS state machine.</summary>
  internal sealed class StateMachineARN
  {
    private static readonly Regex Regex = new Regex(@"^arn:aws:states:([a-zA-Z0-9\-]+):([0-9]+):stateMachine:([a-zA-Z0-9\-]+)$");

    /// <summary>Parses a <see cref="StateMachineARN"/> from the given string.</summary>
    public static StateMachineARN Parse(string arn)
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

      return new StateMachineARN(region, accountId, stateMachineName);
    }

    public StateMachineARN(RegionEndpoint region, long accountId, string stateMachineName)
    {
      Check.NotNull(region, nameof(region));
      Check.That(accountId > 0, "accountId > 0");
      Check.NotNullOrEmpty(stateMachineName, nameof(stateMachineName));

      Region           = region;
      AccountId        = accountId;
      StateMachineName = stateMachineName;
    }

    public RegionEndpoint Region           { get; }
    public long           AccountId        { get; }
    public string         StateMachineName { get; }

    public override string ToString() => $"arn:aws:states:{Region.SystemName}:{AccountId}:stateMachine:{StateMachineName}";

    private bool Equals(StateMachineARN other)
    {
      return Equals(Region, other.Region) &&
             AccountId == other.AccountId &&
             string.Equals(StateMachineName, other.StateMachineName, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;

      return obj is StateMachineARN arn && Equals(arn);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = Region.GetHashCode();

        hashCode = (hashCode * 397) ^ AccountId.GetHashCode();
        hashCode = (hashCode * 397) ^ StateMachineName.GetHashCode();

        return hashCode;
      }
    }
  }
}