using System;
using System.Collections.Concurrent;
using System.IO;

namespace Amazon.S3.Model
{
  /// <summary>A <see cref="IBucket"/> that is stored in-memory.</summary>
  public sealed class InMemoryBucket : IBucket
  {
    private readonly ConcurrentDictionary<string, Entry> entries = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryBucket(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      Name = name;
    }

    public string Name { get; }

    public bool Contains(ObjectId id) => entries.ContainsKey(id.ToString());

    public Stream OpenForReading(ObjectId id)
    {
      if (!entries.TryGetValue(id.ToString(), out var entry))
      {
        throw new InvalidObjectIdException(Name, id);
      }

      return new MemoryStream(entry.Bytes);
    }

    public Stream OpenForWriting(ObjectId id)
    {
      throw new NotImplementedException();
    }

    /// <summary>Encapsulates an entry in this bucket implementation.</summary>
    private sealed class Entry
    {
      public byte[] Bytes { get; } = new byte[0];
    }
  }
}