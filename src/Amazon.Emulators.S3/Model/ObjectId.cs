namespace Amazon.S3.Model
{
  /// <summary>Uniquely identifies an object in an S3 bucket.</summary>
  public readonly ref struct ObjectId
  {
    public ObjectId(string key) : this(key, "latest")
    {
    }

    public ObjectId(string key, string version)
    {
      Check.NotNullOrEmpty(key, nameof(key));

      Key     = key;
      Version = version ?? "latest";
    }

    public readonly string Key;
    public readonly string Version;

    public override string ToString() => $"{Key}@{Version ?? "latest"}";
  }
}