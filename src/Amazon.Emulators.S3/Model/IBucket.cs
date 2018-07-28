namespace Amazon.S3.Model
{
  /// <summary>Represents a bucket capable of storing objects.</summary>
  public interface IBucket
  {
    /// <summary>The name of the bucket.</summary>
    string Name { get; }
  }
}