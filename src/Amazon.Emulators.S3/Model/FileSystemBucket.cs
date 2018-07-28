using System.IO;

namespace Amazon.S3.Model
{
  /// <summary>A <see cref="IBucket"/> that is stored on the file system.</summary>
  public sealed class FileSystemBucket : IBucket
  {
    public FileSystemBucket(string name, string basePath)
    {
      Check.NotNullOrEmpty(name, nameof(name));
      Check.NotNullOrEmpty(basePath, nameof(basePath));

      Name     = name;
      BasePath = basePath;

      CreateDirectoryIfNecessary(basePath);
      CreateDirectoryIfNecessary(Path.Combine(basePath, name));
    }

    public string Name     { get; }
    public string BasePath { get; }

    public bool   Contains(ObjectId       id) => File.Exists(BuildPath(id));
    public Stream OpenForReading(ObjectId id) => File.OpenRead(BuildPath(id));
    public Stream OpenForWriting(ObjectId id) => File.OpenWrite(BuildPath(id));

    private string BuildPath(ObjectId id) => Path.Combine(BasePath, Name, id.ToString());
    
    private static void CreateDirectoryIfNecessary(string path)
    {
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }
    }
  }
}