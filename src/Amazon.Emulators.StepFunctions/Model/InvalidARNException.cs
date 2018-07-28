using System;

namespace Amazon.StepFunctions.Model
{
  internal sealed class InvalidARNException : Exception
  {
    public InvalidARNException(string arn)
      : base($"'{arn}' is not a valid ARN")
    {
    }
  }
}