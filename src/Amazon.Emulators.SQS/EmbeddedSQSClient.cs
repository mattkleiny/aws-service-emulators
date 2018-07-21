using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Embedded;
using Amazon.Runtime;
using Amazon.SQS.Model;

namespace Amazon.SQS
{
  internal sealed class EmbeddedSQSClient : IAmazonSQS
  {
    private readonly EmbeddedSQS embedded;

    public EmbeddedSQSClient(EmbeddedSQS embedded)
    {
      Check.NotNull(embedded, nameof(embedded));

      this.embedded = embedded;
    }

    public IClientConfig Config { get; } = EmbeddedClientConfig.Instance;

    public Task<Dictionary<string, string>> GetAttributesAsync(string queueUrl)
    {
      throw new NotSupportedException();
    }

    public Task SetAttributesAsync(string queueUrl, Dictionary<string, string> attributes)
    {
      throw new NotSupportedException();
    }

    public Task<string> AuthorizeS3ToSendMessageAsync(string queueUrl, string bucket)
    {
      throw new NotSupportedException();
    }

    public Task<AddPermissionResponse> AddPermissionAsync(string queueUrl, string label, List<string> awsAccountIds, List<string> actions, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<AddPermissionResponse> AddPermissionAsync(AddPermissionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(string queueUrl, string receiptHandle, int visibilityTimeout, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(ChangeMessageVisibilityRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatchAsync(string queueUrl, List<ChangeMessageVisibilityBatchRequestEntry> entries, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatchAsync(ChangeMessageVisibilityBatchRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<CreateQueueResponse> CreateQueueAsync(string queueName, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<CreateQueueResponse> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<DeleteMessageResponse> DeleteMessageAsync(string queueUrl, string receiptHandle, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<DeleteMessageResponse> DeleteMessageAsync(DeleteMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<DeleteMessageBatchResponse> DeleteMessageBatchAsync(string queueUrl, List<DeleteMessageBatchRequestEntry> entries, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<DeleteMessageBatchResponse> DeleteMessageBatchAsync(DeleteMessageBatchRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<DeleteQueueResponse> DeleteQueueAsync(string queueUrl, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<DeleteQueueResponse> DeleteQueueAsync(DeleteQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<GetQueueAttributesResponse> GetQueueAttributesAsync(string queueUrl, List<string> attributeNames, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<GetQueueAttributesResponse> GetQueueAttributesAsync(GetQueueAttributesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<GetQueueUrlResponse> GetQueueUrlAsync(string queueName, CancellationToken cancellationToken = new CancellationToken())
    {
      return GetQueueUrlAsync(new GetQueueUrlRequest(queueName), cancellationToken);
    }

    public Task<GetQueueUrlResponse> GetQueueUrlAsync(GetQueueUrlRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      if (!embedded.TryGetQueueByName(request.QueueName, out var queue))
      {
        return Task.FromResult(new GetQueueUrlResponse
        {
          HttpStatusCode = HttpStatusCode.NotFound
        });
      }

      return Task.FromResult(new GetQueueUrlResponse
      {
        QueueUrl       = queue.Url,
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public Task<ListDeadLetterSourceQueuesResponse> ListDeadLetterSourceQueuesAsync(ListDeadLetterSourceQueuesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ListQueuesResponse> ListQueuesAsync(string queueNamePrefix, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ListQueuesResponse> ListQueuesAsync(ListQueuesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ListQueueTagsResponse> ListQueueTagsAsync(ListQueueTagsRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<PurgeQueueResponse> PurgeQueueAsync(string queueUrl, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<PurgeQueueResponse> PurgeQueueAsync(PurgeQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ReceiveMessageResponse> ReceiveMessageAsync(string queueUrl, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<RemovePermissionResponse> RemovePermissionAsync(string queueUrl, string label, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<RemovePermissionResponse> RemovePermissionAsync(RemovePermissionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody, CancellationToken cancellationToken = new CancellationToken())
    {
      return SendMessageAsync(new SendMessageRequest(queueUrl, messageBody), cancellationToken);
    }

    public Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      if (!embedded.TryGetQueueByUrl(request.QueueUrl, out var queue))
      {
        return Task.FromResult(new SendMessageResponse
        {
          HttpStatusCode = HttpStatusCode.NotFound
        });
      }

      var message = new Message
      {
        MessageId  = Guid.NewGuid().ToString(),
        Body       = request.MessageBody,
        Attributes = request.MessageAttributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.StringValue)
      };

      queue.Enqueue(message);

      return Task.FromResult(new SendMessageResponse
      {
        MessageId      = message.MessageId,
        HttpStatusCode = HttpStatusCode.OK
      });
    }

    public Task<SendMessageBatchResponse> SendMessageBatchAsync(string queueUrl, List<SendMessageBatchRequestEntry> entries, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<SendMessageBatchResponse> SendMessageBatchAsync(SendMessageBatchRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<SetQueueAttributesResponse> SetQueueAttributesAsync(string queueUrl, Dictionary<string, string> attributes, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<SetQueueAttributesResponse> SetQueueAttributesAsync(SetQueueAttributesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<TagQueueResponse> TagQueueAsync(TagQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public Task<UntagQueueResponse> UntagQueueAsync(UntagQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public void Dispose()
    {
      // no-op
    }
  }
}