using System;

namespace Amazon.SQS.Model
{
  internal sealed class InvalidQueueUrlException : Exception
  {
    public InvalidQueueUrlException(string url)
      : base($"'{url}' is not a valid SQS queue URL.")
    {
    }
  }
}