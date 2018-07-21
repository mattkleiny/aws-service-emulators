using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Amazon.Emulators.Embedded
{
  /// <summary>Base class for any <see cref="Amazon.SQS.IAmazonSQS"/> decorator.</summary>
  public abstract class DecoratedAmazonSQS : IAmazonSQS
  {
    private readonly IAmazonSQS target;

    protected DecoratedAmazonSQS(IAmazonSQS target)
    {
      this.target = target;
    }

    public virtual IClientConfig Config => target.Config;

    public virtual Task<Dictionary<string, string>> GetAttributesAsync(string queueUrl)
      => target.GetAttributesAsync(queueUrl);

    public virtual Task SetAttributesAsync(string queueUrl, Dictionary<string, string> attributes)
      => target.SetAttributesAsync(queueUrl, attributes);

    public virtual Task<string> AuthorizeS3ToSendMessageAsync(string queueUrl, string bucket)
      => target.AuthorizeS3ToSendMessageAsync(queueUrl, bucket);

    public virtual Task<AddPermissionResponse> AddPermissionAsync(string queueUrl, string label, List<string> awsAccountIds, List<string> actions, CancellationToken cancellationToken = new CancellationToken())
      => target.AddPermissionAsync(queueUrl, label, awsAccountIds, actions, cancellationToken);

    public virtual Task<AddPermissionResponse> AddPermissionAsync(AddPermissionRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.AddPermissionAsync(request, cancellationToken);

    public virtual Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(string queueUrl, string receiptHandle, int visibilityTimeout, CancellationToken cancellationToken = new CancellationToken())
      => target.ChangeMessageVisibilityAsync(queueUrl, receiptHandle, visibilityTimeout, cancellationToken);

    public virtual Task<ChangeMessageVisibilityResponse> ChangeMessageVisibilityAsync(ChangeMessageVisibilityRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.ChangeMessageVisibilityAsync(request, cancellationToken);

    public virtual Task<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatchAsync(string queueUrl, List<ChangeMessageVisibilityBatchRequestEntry> entries, CancellationToken cancellationToken = new CancellationToken())
      => target.ChangeMessageVisibilityBatchAsync(queueUrl, entries, cancellationToken);

    public virtual Task<ChangeMessageVisibilityBatchResponse> ChangeMessageVisibilityBatchAsync(ChangeMessageVisibilityBatchRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.ChangeMessageVisibilityBatchAsync(request, cancellationToken);

    public virtual Task<CreateQueueResponse> CreateQueueAsync(string queueName, CancellationToken cancellationToken = new CancellationToken())
      => target.CreateQueueAsync(queueName, cancellationToken);

    public virtual Task<CreateQueueResponse> CreateQueueAsync(CreateQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.CreateQueueAsync(request, cancellationToken);

    public virtual Task<DeleteMessageResponse> DeleteMessageAsync(string queueUrl, string receiptHandle, CancellationToken cancellationToken = new CancellationToken())
      => target.DeleteMessageAsync(queueUrl, receiptHandle, cancellationToken);

    public virtual Task<DeleteMessageResponse> DeleteMessageAsync(DeleteMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.DeleteMessageAsync(request, cancellationToken);

    public virtual Task<DeleteMessageBatchResponse> DeleteMessageBatchAsync(string queueUrl, List<DeleteMessageBatchRequestEntry> entries, CancellationToken cancellationToken = new CancellationToken())
      => target.DeleteMessageBatchAsync(queueUrl, entries, cancellationToken);

    public virtual Task<DeleteMessageBatchResponse> DeleteMessageBatchAsync(DeleteMessageBatchRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.DeleteMessageBatchAsync(request, cancellationToken);

    public virtual Task<DeleteQueueResponse> DeleteQueueAsync(string queueUrl, CancellationToken cancellationToken = new CancellationToken())
      => target.DeleteQueueAsync(queueUrl, cancellationToken);

    public virtual Task<DeleteQueueResponse> DeleteQueueAsync(DeleteQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.DeleteQueueAsync(request, cancellationToken);

    public virtual Task<GetQueueAttributesResponse> GetQueueAttributesAsync(string queueUrl, List<string> attributeNames, CancellationToken cancellationToken = new CancellationToken())
      => target.GetQueueAttributesAsync(queueUrl, attributeNames, cancellationToken);

    public virtual Task<GetQueueAttributesResponse> GetQueueAttributesAsync(GetQueueAttributesRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.GetQueueAttributesAsync(request, cancellationToken);

    public virtual Task<GetQueueUrlResponse> GetQueueUrlAsync(string queueName, CancellationToken cancellationToken = new CancellationToken())
      => target.GetQueueUrlAsync(queueName, cancellationToken);

    public virtual Task<GetQueueUrlResponse> GetQueueUrlAsync(GetQueueUrlRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.GetQueueUrlAsync(request, cancellationToken);

    public virtual Task<ListDeadLetterSourceQueuesResponse> ListDeadLetterSourceQueuesAsync(ListDeadLetterSourceQueuesRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.ListDeadLetterSourceQueuesAsync(request, cancellationToken);

    public virtual Task<ListQueuesResponse> ListQueuesAsync(string queueNamePrefix, CancellationToken cancellationToken = new CancellationToken())
      => target.ListQueuesAsync(queueNamePrefix, cancellationToken);

    public virtual Task<ListQueuesResponse> ListQueuesAsync(ListQueuesRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.ListQueuesAsync(request, cancellationToken);

    public virtual Task<ListQueueTagsResponse> ListQueueTagsAsync(ListQueueTagsRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.ListQueueTagsAsync(request, cancellationToken);

    public virtual Task<PurgeQueueResponse> PurgeQueueAsync(string queueUrl, CancellationToken cancellationToken = new CancellationToken())
      => target.PurgeQueueAsync(queueUrl, cancellationToken);

    public virtual Task<PurgeQueueResponse> PurgeQueueAsync(PurgeQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.PurgeQueueAsync(request, cancellationToken);

    public virtual Task<ReceiveMessageResponse> ReceiveMessageAsync(string queueUrl, CancellationToken cancellationToken = new CancellationToken())
      => target.ReceiveMessageAsync(queueUrl, cancellationToken);

    public virtual Task<ReceiveMessageResponse> ReceiveMessageAsync(ReceiveMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.ReceiveMessageAsync(request, cancellationToken);

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(string queueUrl, string label, CancellationToken cancellationToken = new CancellationToken())
      => target.RemovePermissionAsync(queueUrl, label, cancellationToken);

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(RemovePermissionRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.RemovePermissionAsync(request, cancellationToken);

    public virtual Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody, CancellationToken cancellationToken = new CancellationToken())
      => target.SendMessageAsync(queueUrl, messageBody, cancellationToken);

    public virtual Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.SendMessageAsync(request, cancellationToken);

    public virtual Task<SendMessageBatchResponse> SendMessageBatchAsync(string queueUrl, List<SendMessageBatchRequestEntry> entries, CancellationToken cancellationToken = new CancellationToken())
      => target.SendMessageBatchAsync(queueUrl, entries, cancellationToken);

    public virtual Task<SendMessageBatchResponse> SendMessageBatchAsync(SendMessageBatchRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.SendMessageBatchAsync(request, cancellationToken);

    public virtual Task<SetQueueAttributesResponse> SetQueueAttributesAsync(string queueUrl, Dictionary<string, string> attributes, CancellationToken cancellationToken = new CancellationToken())
      => target.SetQueueAttributesAsync(queueUrl, attributes, cancellationToken);

    public virtual Task<SetQueueAttributesResponse> SetQueueAttributesAsync(SetQueueAttributesRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.SetQueueAttributesAsync(request, cancellationToken);

    public virtual Task<TagQueueResponse> TagQueueAsync(TagQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.TagQueueAsync(request, cancellationToken);

    public virtual Task<UntagQueueResponse> UntagQueueAsync(UntagQueueRequest request, CancellationToken cancellationToken = new CancellationToken())
      => target.UntagQueueAsync(request, cancellationToken);

    public virtual void Dispose() => target.Dispose();
  }
}