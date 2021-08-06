using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.Runtime;
using Amazon.Runtime.SharedInterfaces;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Amazon.SNS.Internal
{
  internal abstract class AmazonSNSBase : IAmazonSimpleNotificationService
  {
    public IClientConfig Config { get; } = EmptyClientConfig.Instance;

    public virtual Task<string> SubscribeQueueAsync(string topicArn, ICoreAmazonSQS sqsClient, string sqsQueueUrl)
    {
      throw new NotSupportedException();
    }

    public virtual Task<IDictionary<string, string>> SubscribeQueueToTopicsAsync(IList<string> topicArns, ICoreAmazonSQS sqsClient, string sqsQueueUrl)
    {
      throw new NotSupportedException();
    }

    public virtual Task<Topic> FindTopicAsync(string topicName)
    {
      throw new NotSupportedException();
    }

    public virtual Task AuthorizeS3ToPublishAsync(string topicArn, string bucket)
    {
      throw new NotSupportedException();
    }

    public virtual Task<AddPermissionResponse> AddPermissionAsync(string topicArn, string label, List<string> awsAccountId, List<string> actionName, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<AddPermissionResponse> AddPermissionAsync(AddPermissionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CheckIfPhoneNumberIsOptedOutResponse> CheckIfPhoneNumberIsOptedOutAsync(CheckIfPhoneNumberIsOptedOutRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ConfirmSubscriptionResponse> ConfirmSubscriptionAsync(string topicArn, string token, string authenticateOnUnsubscribe, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ConfirmSubscriptionResponse> ConfirmSubscriptionAsync(string topicArn, string token, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ConfirmSubscriptionResponse> ConfirmSubscriptionAsync(ConfirmSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreatePlatformApplicationResponse> CreatePlatformApplicationAsync(CreatePlatformApplicationRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreatePlatformEndpointResponse> CreatePlatformEndpointAsync(CreatePlatformEndpointRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateTopicResponse> CreateTopicAsync(string name, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateTopicResponse> CreateTopicAsync(CreateTopicRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteEndpointResponse> DeleteEndpointAsync(DeleteEndpointRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeletePlatformApplicationResponse> DeletePlatformApplicationAsync(DeletePlatformApplicationRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteTopicResponse> DeleteTopicAsync(string topicArn, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteTopicResponse> DeleteTopicAsync(DeleteTopicRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetEndpointAttributesResponse> GetEndpointAttributesAsync(GetEndpointAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetPlatformApplicationAttributesResponse> GetPlatformApplicationAttributesAsync(GetPlatformApplicationAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetSMSAttributesResponse> GetSMSAttributesAsync(GetSMSAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetSubscriptionAttributesResponse> GetSubscriptionAttributesAsync(string subscriptionArn, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetSubscriptionAttributesResponse> GetSubscriptionAttributesAsync(GetSubscriptionAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetTopicAttributesResponse> GetTopicAttributesAsync(string topicArn, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetTopicAttributesResponse> GetTopicAttributesAsync(GetTopicAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListEndpointsByPlatformApplicationResponse> ListEndpointsByPlatformApplicationAsync(ListEndpointsByPlatformApplicationRequest request,
      CancellationToken                                                                                                                               cancellationToken = new())
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListPhoneNumbersOptedOutResponse> ListPhoneNumbersOptedOutAsync(ListPhoneNumbersOptedOutRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListPlatformApplicationsResponse> ListPlatformApplicationsAsync(CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListPlatformApplicationsResponse> ListPlatformApplicationsAsync(ListPlatformApplicationsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListSubscriptionsResponse> ListSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListSubscriptionsResponse> ListSubscriptionsAsync(string nextToken, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListSubscriptionsResponse> ListSubscriptionsAsync(ListSubscriptionsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListSubscriptionsByTopicResponse> ListSubscriptionsByTopicAsync(string topicArn, string nextToken, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListSubscriptionsByTopicResponse> ListSubscriptionsByTopicAsync(string topicArn, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListSubscriptionsByTopicResponse> ListSubscriptionsByTopicAsync(ListSubscriptionsByTopicRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListTopicsResponse> ListTopicsAsync(CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListTopicsResponse> ListTopicsAsync(string nextToken, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListTopicsResponse> ListTopicsAsync(ListTopicsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<OptInPhoneNumberResponse> OptInPhoneNumberAsync(OptInPhoneNumberRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<PublishResponse> PublishAsync(string topicArn, string message, CancellationToken cancellationToken = default)
    {
      return PublishAsync(new PublishRequest(topicArn, message), cancellationToken);
    }

    public Task<PublishResponse> PublishAsync(string topicArn, string message, string subject, CancellationToken cancellationToken = default)
    {
      return PublishAsync(new PublishRequest(topicArn, message, subject), cancellationToken);
    }

    public virtual Task<PublishResponse> PublishAsync(PublishRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(string topicArn, string label, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(RemovePermissionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetEndpointAttributesResponse> SetEndpointAttributesAsync(SetEndpointAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetPlatformApplicationAttributesResponse> SetPlatformApplicationAttributesAsync(SetPlatformApplicationAttributesRequest request,
      CancellationToken                                                                                                                         cancellationToken = new())
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetSMSAttributesResponse> SetSMSAttributesAsync(SetSMSAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetSubscriptionAttributesResponse> SetSubscriptionAttributesAsync(string subscriptionArn, string attributeName, string attributeValue,
      CancellationToken                                                                          cancellationToken = new())
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetSubscriptionAttributesResponse> SetSubscriptionAttributesAsync(SetSubscriptionAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetTopicAttributesResponse> SetTopicAttributesAsync(string topicArn, string attributeName, string attributeValue, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SetTopicAttributesResponse> SetTopicAttributesAsync(SetTopicAttributesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SubscribeResponse> SubscribeAsync(string topicArn, string protocol, string endpoint, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<SubscribeResponse> SubscribeAsync(SubscribeRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UnsubscribeResponse> UnsubscribeAsync(string subscriptionArn, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UnsubscribeResponse> UnsubscribeAsync(UnsubscribeRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual void Dispose()
    {
      // no-op
    }
  }
}