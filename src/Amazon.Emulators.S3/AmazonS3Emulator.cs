using System;
using System.Collections.Concurrent;
using Amazon.Emulators;
using Amazon.S3.Internal;
using Amazon.S3.Model;

namespace Amazon.S3
{
  /// <summary>A factory for <see cref="IBucket"/>s, given the bucket name.</summary>
  public delegate IBucket BucketFactory(string name);

  /// <summary>An emulator for Amazon's Simple Storage Service (S3).</summary>
  public sealed class AmazonS3Emulator : IAmazonServiceEmulator<IAmazonS3>
  {
    private readonly ConcurrentDictionary<string, IBucket> bucketsByName = new(StringComparer.OrdinalIgnoreCase);

    private readonly BucketFactory factory;

    public AmazonS3Emulator(BucketFactory factory)
    {
      Check.NotNull(factory, nameof(factory));

      this.factory = factory;

      Client = new EmulatedAmazonS3(this);
    }

    public IAmazonS3 Client { get; }

    /// <summary>Retrieves or creates a new <see cref="IBucket"/> with the given name.</summary>
    internal IBucket GetOrCreateBucket(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      return bucketsByName.GetOrAdd(name, _ => factory(_));
    }
  }
}