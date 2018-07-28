namespace Amazon.S3.Model
{
  /// <summary>A <see cref="IBucket"/> that is stored in-memory.</summary>
  public sealed class InMemoryBucket : IBucket
  {
    public InMemoryBucket(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      Name = name;
    }

    public string Name { get; }
  }
}