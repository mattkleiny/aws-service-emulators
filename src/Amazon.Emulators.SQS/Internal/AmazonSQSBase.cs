using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.Runtime;
using Amazon.SQS.Model;

namespace Amazon.SQS.Internal
{
  /// <summary>Base class for any <see cref="Amazon.SQS.IAmazonSQS"/> implementations, to help separate plumbing from intent.</summary>
  internal abstract class AmazonSQSBase : IAmazonSQS
  {
    public IClientConfig Config { get; } = EmptyClientConfig.Instance;

    public virtual Task<Dictionary<string, string>> GetAttributesAsync(string queueUrl)
    {
      throw new NotSupportedException();
    }

    public virtual Task SetAttributesAsync(string queueUrl, Dictionary<string, string> attributes)
    {
      throw new NotSupportedException();
    }

    public virtual Task<string> AuthorizeS3ToSendMessageAsync(string queueUrl, string bucket)
    {
      throw new NotSupportedException();
    }

    public virtual Task<AddPermissionResponse> AddPermissionAsync(string queueUrl, string label, List<string> awsAccountIds, List<string> actions, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<AddPermissionResponse> AddPermissionAsync(AddPermissionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(string queueUrl, string receiptHandle, int visibilityTimeout, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(ChangeMessageVisibilityRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatchAsync(string queueUrl, List<ChangeMessageVisibilityBatchRequestEntry> entries,
      CancellationToken                                                                                cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatchAsync(ChangeMessageVisibilityBatchRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateQueueResponse> CreateQueueAsync(string queueName, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateQueueResponse> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<DeleteMessageResponse> DeleteMessageAsync(string queueUrl, string receiptHandle, CancellationToken cancellationToken = default)
    {
      return DeleteMessageAsync(new DeleteMessageRequest(queueUrl, receiptHandle), cancellationToken);
    }

    public virtual Task<DeleteMessageResponse> DeleteMessageAsync(DeleteMessageRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteMessageBatchResponse> DeleteMessageBatchAsync(string queueUrl, List<DeleteMessageBatchRequestEntry> entries, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteMessageBatchResponse> DeleteMessageBatchAsync(DeleteMessageBatchRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteQueueResponse> DeleteQueueAsync(string queueUrl, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteQueueResponse> DeleteQueueAsync(DeleteQueueRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<GetQueueAttributesResponse> GetQueueAttributesAsync(string queueUrl, List<string> attributeNames, CancellationToken cancellationToken = default)
    {
      var request = new GetQueueAttributesRequest
      {
        QueueUrl       = queueUrl,
        AttributeNames = attributeNames,
      };

      return GetQueueAttributesAsync(request, cancellationToken);
    }

    public virtual Task<GetQueueAttributesResponse> GetQueueAttributesAsync(GetQueueAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<GetQueueUrlResponse> GetQueueUrlAsync(string queueName, CancellationToken cancellationToken = default)
    {
      Check.NotNullOrEmpty(queueName, nameof(queueName));

      return GetQueueUrlAsync(new GetQueueUrlRequest(queueName), cancellationToken);
    }

    public virtual Task<GetQueueUrlResponse> GetQueueUrlAsync(GetQueueUrlRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListDeadLetterSourceQueuesResponse> ListDeadLetterSourceQueuesAsync(ListDeadLetterSourceQueuesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListQueuesResponse> ListQueuesAsync(string queueNamePrefix, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListQueuesResponse> ListQueuesAsync(ListQueuesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListQueueTagsResponse> ListQueueTagsAsync(ListQueueTagsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<PurgeQueueResponse> PurgeQueueAsync(string queueUrl, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<PurgeQueueResponse> PurgeQueueAsync(PurgeQueueRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<ReceiveMessageResponse> ReceiveMessageAsync(string queueUrl, CancellationToken cancellationToken = default)
    {
      Check.NotNullOrEmpty(queueUrl, nameof(queueUrl));

      return ReceiveMessageAsync(new ReceiveMessageRequest(queueUrl), cancellationToken);
    }

    public virtual Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(string queueUrl, string label, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(RemovePermissionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody, CancellationToken cancellationToken = default)
    {
      Check.NotNullOrEmpty(queueUrl, nameof(queueUrl));
      Check.NotNullOrEmpty(messageBody, nameof(messageBody));

      return SendMessageAsync(new SendMessageRequest(queueUrl, messageBody), cancellationToken);
    }

    public virtual Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SendMessageBatchResponse> SendMessageBatchAsync(string queueUrl, List<SendMessageBatchRequestEntry> entries, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SendMessageBatchResponse> SendMessageBatchAsync(SendMessageBatchRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetQueueAttributesResponse> SetQueueAttributesAsync(string queueUrl, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetQueueAttributesResponse> SetQueueAttributesAsync(SetQueueAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<TagQueueResponse> TagQueueAsync(TagQueueRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UntagQueueResponse> UntagQueueAsync(UntagQueueRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual void Dispose()
    {
      // no-op
    }
  }
}