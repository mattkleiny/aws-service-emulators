namespace Amazon.S3.Model
{
  /// <summary>A <see cref="IBucket"/> that is stored on the file system.</summary>
  public sealed class FileSystemBucket : IBucket
  {
    public FileSystemBucket(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      Name = name;
    }

    public string Name { get; }
  }
}