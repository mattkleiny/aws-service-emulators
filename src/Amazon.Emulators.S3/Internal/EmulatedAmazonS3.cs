namespace Amazon.S3.Internal
{
  internal sealed class EmulatedAmazonS3 : AmazonS3Base
  {
    private readonly AmazonS3Emulator emulator;

    public EmulatedAmazonS3(AmazonS3Emulator emulator)
    {
      Check.NotNull(emulator, nameof(emulator));

      this.emulator = emulator;
    }
  }
}