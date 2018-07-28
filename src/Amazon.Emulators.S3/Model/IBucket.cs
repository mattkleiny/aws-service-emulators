using System.IO;

namespace Amazon.S3.Model
{
  /// <summary>Represents a bucket capable of storing objects.</summary>
  public interface IBucket
  {
    /// <summary>The name of the bucket.</summary>
    string Name { get; }

    /// <summary>Determines if the bucket contains the given object.</summary>
    bool Contains(ObjectId id);

    /// <summary>Attempts to open a stream for reading the given object.</summary>
    Stream OpenForReading(ObjectId id);

    /// <summary>Attempts to open a stream for writing the given object.</summary>
    Stream OpenForWriting(ObjectId id);
  }
}