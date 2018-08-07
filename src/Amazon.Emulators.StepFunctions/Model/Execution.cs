using System;
using System.Threading.Tasks;
using Amazon.StepFunction.Hosting;

namespace Amazon.StepFunction.Model
{
  /// <summary>Encapsulates an execution that has occurred in this emulator.</summary>
  internal sealed class Execution
  {
    public Execution(Task<StepFunctionHost.Result> task, ExecutionARN arn)
    {
      Check.NotNull(task, nameof(task));
      Check.NotNull(arn, nameof(arn));

      ARN    = arn;
      Status = ExecutionState.Processing;

      task.ContinueWith(parent =>
      {
        if (parent.IsFaulted)
        {
          Status    = ExecutionState.Failed;
          Exception = parent.Exception;
        }
        else
        {
          Result = parent.Result;

          if (Result.IsFailure)
          {
            Status    = ExecutionState.Failed;
            Exception = parent.Result.Exception;
          }
          else
          {
            Status = ExecutionState.Completed;
          }
        }

        StopDate = DateTime.Now;
      });
    }

    public ExecutionARN   ARN       { get; }
    public DateTime       StartDate { get; } = DateTime.Now;
    public DateTime?      StopDate  { get; private set; }
    public ExecutionState Status    { get; private set; }
    public Exception      Exception { get; private set; }

    public StepFunctionHost.Result Result { get; private set; }

    public override string ToString() => $"{ARN} - {Status} (started at {StartDate})";
  }
}