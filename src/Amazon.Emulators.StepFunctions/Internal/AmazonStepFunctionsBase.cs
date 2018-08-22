using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators;
using Amazon.Runtime;
using Amazon.StepFunctions.Model;

namespace Amazon.StepFunctions.Internal
{
  /// <summary>Base class for any <see cref="IAmazonStepFunctions"/> implementations, to help separate plumbing from intent.</summary>
  internal abstract class AmazonStepFunctionsBase : IAmazonStepFunctions
  {
    public IClientConfig Config { get; } = EmptyClientConfig.Instance;

    public virtual Task<CreateActivityResponse> CreateActivityAsync(CreateActivityRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<CreateStateMachineResponse> CreateStateMachineAsync(CreateStateMachineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteActivityResponse> DeleteActivityAsync(DeleteActivityRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<DeleteStateMachineResponse> DeleteStateMachineAsync(DeleteStateMachineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<DescribeActivityResponse> DescribeActivityAsync(DescribeActivityRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<DescribeExecutionResponse> DescribeExecutionAsync(DescribeExecutionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<DescribeStateMachineResponse> DescribeStateMachineAsync(DescribeStateMachineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<DescribeStateMachineForExecutionResponse> DescribeStateMachineForExecutionAsync(DescribeStateMachineForExecutionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetActivityTaskResponse> GetActivityTaskAsync(GetActivityTaskRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<GetExecutionHistoryResponse> GetExecutionHistoryAsync(GetExecutionHistoryRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListActivitiesResponse> ListActivitiesAsync(ListActivitiesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListExecutionsResponse> ListExecutionsAsync(ListExecutionsRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<ListStateMachinesResponse> ListStateMachinesAsync(ListStateMachinesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<SendTaskFailureResponse> SendTaskFailureAsync(SendTaskFailureRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<SendTaskHeartbeatResponse> SendTaskHeartbeatAsync(SendTaskHeartbeatRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<SendTaskSuccessResponse> SendTaskSuccessAsync(SendTaskSuccessRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<StartExecutionResponse> StartExecutionAsync(StartExecutionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<StopExecutionResponse> StopExecutionAsync(StopExecutionRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public virtual Task<UpdateStateMachineResponse> UpdateStateMachineAsync(UpdateStateMachineRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
      throw new NotSupportedException();
    }

    public void Dispose()
    {
      // no-op
    }
  }
}