using System;

namespace Amazon.S3.Model
{
  public sealed class InvalidObjectIdException : Exception
  {
    public InvalidObjectIdException(string bucketName, in ObjectId id)
      : base($"The object {id.ToString()} does not existing in bucket {bucketName}")
    {
    }
  }
}