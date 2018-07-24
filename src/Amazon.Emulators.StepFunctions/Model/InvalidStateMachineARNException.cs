using System;

namespace Amazon.StepFunctions.Model
{
  public sealed class InvalidStateMachineARNException : Exception
  {
    public InvalidStateMachineARNException(string arn)
      : base($"'{arn}' is not a valid state machine ARN")
    {
    }
  }
}