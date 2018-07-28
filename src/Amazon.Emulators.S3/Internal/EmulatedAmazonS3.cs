using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;

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

    public override Task<GetObjectResponse> GetObjectAsync(GetObjectRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var bucket   = emulator.GetOrCreateBucket(request.BucketName);
      var objectId = new ObjectId(request.Key, request.VersionId);

      if (!bucket.Contains(objectId))
      {
        return Task.FromResult(new GetObjectResponse
        {
          HttpStatusCode = HttpStatusCode.NotFound
        });
      }

      return Task.FromResult(new GetObjectResponse
      {
        BucketName     = request.BucketName,
        Key            = request.Key,
        VersionId      = request.VersionId ?? "latest",
        ResponseStream = bucket.OpenForReading(objectId),
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public override async Task<PutObjectResponse> PutObjectAsync(PutObjectRequest request, CancellationToken cancellationToken = default)
    {
      Check.NotNull(request, nameof(request));

      var bucket = emulator.GetOrCreateBucket(request.BucketName);

      try
      {
        if (!string.IsNullOrEmpty(request.ContentBody))
        {
          request.AutoCloseStream = true;
          request.InputStream     = new MemoryStream(Encoding.UTF8.GetBytes(request.ContentBody));
        }

        using (var stream = bucket.OpenForWriting(new ObjectId(request.Key)))
        {
          await request.InputStream.CopyToAsync(stream);
        }
      }
      finally
      {
        if (request.AutoCloseStream)
        {
          request.InputStream?.Close();
        }
      }

      return new PutObjectResponse
      {
        HttpStatusCode = HttpStatusCode.OK
      };
    }
  }
}