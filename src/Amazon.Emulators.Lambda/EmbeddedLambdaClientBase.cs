using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Embedded;
using Amazon.Lambda.Model;
using Amazon.Runtime;

namespace Amazon.Lambda
{
  /// <summary>Base class for any <see cref="IAmazonLambda"/> implementations, to help separate plumbing from intent.</summary>
  internal abstract class EmbeddedLambdaClientBase : IAmazonLambda
  {
    public IClientConfig Config { get; } = EmbeddedClientConfig.Instance;

    public virtual Task<AddPermissionResponse> AddPermissionAsync(AddPermissionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateAliasResponse> CreateAliasAsync(CreateAliasRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateEventSourceMappingResponse> CreateEventSourceMappingAsync(CreateEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateFunctionResponse> CreateFunctionAsync(CreateFunctionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteAliasResponse> DeleteAliasAsync(DeleteAliasRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteEventSourceMappingResponse> DeleteEventSourceMappingAsync(DeleteEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<DeleteFunctionResponse> DeleteFunctionAsync(string functionName, CancellationToken cancellationToken = default)
    {
      return DeleteFunctionAsync(new DeleteFunctionRequest {FunctionName = functionName}, cancellationToken);
    }

    public virtual Task<DeleteFunctionResponse> DeleteFunctionAsync(DeleteFunctionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteFunctionConcurrencyResponse> DeleteFunctionConcurrencyAsync(DeleteFunctionConcurrencyRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetAccountSettingsResponse> GetAccountSettingsAsync(GetAccountSettingsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetAliasResponse> GetAliasAsync(GetAliasRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetEventSourceMappingResponse> GetEventSourceMappingAsync(GetEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<GetFunctionResponse> GetFunctionAsync(string functionName, CancellationToken cancellationToken = default)
    {
      return GetFunctionAsync(new GetFunctionRequest {FunctionName = functionName}, cancellationToken);
    }

    public virtual Task<GetFunctionResponse> GetFunctionAsync(GetFunctionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetFunctionConfigurationResponse> GetFunctionConfigurationAsync(string functionName, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetFunctionConfigurationResponse> GetFunctionConfigurationAsync(GetFunctionConfigurationRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetPolicyResponse> GetPolicyAsync(GetPolicyRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<InvokeResponse> InvokeAsync(InvokeRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<InvokeAsyncResponse> InvokeAsyncAsync(InvokeAsyncRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListAliasesResponse> ListAliasesAsync(ListAliasesRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListEventSourceMappingsResponse> ListEventSourceMappingsAsync(ListEventSourceMappingsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public Task<ListFunctionsResponse> ListFunctionsAsync(CancellationToken cancellationToken = default)
    {
      return ListFunctionsAsync(new ListFunctionsRequest(), cancellationToken);
    }

    public virtual Task<ListFunctionsResponse> ListFunctionsAsync(ListFunctionsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListTagsResponse> ListTagsAsync(ListTagsRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListVersionsByFunctionResponse> ListVersionsByFunctionAsync(ListVersionsByFunctionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<PublishVersionResponse> PublishVersionAsync(PublishVersionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<PutFunctionConcurrencyResponse> PutFunctionConcurrencyAsync(PutFunctionConcurrencyRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<RemovePermissionResponse> RemovePermissionAsync(RemovePermissionRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<TagResourceResponse> TagResourceAsync(TagResourceRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UntagResourceResponse> UntagResourceAsync(UntagResourceRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UpdateAliasResponse> UpdateAliasAsync(UpdateAliasRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UpdateEventSourceMappingResponse> UpdateEventSourceMappingAsync(UpdateEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UpdateFunctionCodeResponse> UpdateFunctionCodeAsync(UpdateFunctionCodeRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public virtual Task<UpdateFunctionConfigurationResponse> UpdateFunctionConfigurationAsync(UpdateFunctionConfigurationRequest request, CancellationToken cancellationToken = default)
    {
      throw new NotSupportedException();
    }

    public void Dispose()
    {
      // no-op
    }
  }
}