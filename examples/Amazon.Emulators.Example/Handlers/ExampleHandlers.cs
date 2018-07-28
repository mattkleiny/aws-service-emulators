using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Hosting;
using Amazon.Lambda.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Microsoft.Extensions.Logging;

namespace Amazon.Emulators.Example.Handlers
{
  public sealed class ExampleHandlers
  {
    private const string QueueName = "test-queue";

    private readonly ILogger<ExampleHandlers> logger;
    private readonly IAmazonSQS               sqs;
    private readonly IAmazonLambda            lambda;
    private readonly IAmazonStepFunctions     stepFunctions;

    public ExampleHandlers(ILogger<ExampleHandlers> logger, IAmazonSQS sqs, IAmazonLambda lambda, IAmazonStepFunctions stepFunctions)
    {
      Check.NotNull(logger, nameof(logger));
      Check.NotNull(sqs, nameof(sqs));
      Check.NotNull(lambda, nameof(lambda));
      Check.NotNull(stepFunctions, nameof(stepFunctions));

      this.logger        = logger;
      this.sqs           = sqs;
      this.lambda        = lambda;
      this.stepFunctions = stepFunctions;
    }

    [LambdaFunction("entry-point")]
    public async Task<string> EntryPointAsync()
    {
      await lambda.InvokeAsync(new InvokeRequest
      {
        FunctionName = "process-queue"
      });

      return "OK";
    }

    [LambdaFunction("process-queue")]
    public async Task<string> ProcessQueueAsync(CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName, cancellationToken)).QueueUrl;

      for (var i = 0; i < 100; i++)
      {
        await sqs.SendMessageAsync(queueUrl, "world", cancellationToken);
      }

      while (!cancellationToken.IsCancellationRequested)
      {
        var request = new ReceiveMessageRequest
        {
          QueueUrl            = queueUrl,
          MaxNumberOfMessages = 10,
          WaitTimeSeconds     = 5
        };

        var batch = await sqs.ReceiveMessageAsync(request, cancellationToken);
        if (batch.Messages.Count == 0) break; // received an empty batch? go ahead and complete

        foreach (var message in batch.Messages)
        {
          var invocation = new InvokeRequest
          {
            FunctionName = "launch-stepfunction",
            Payload      = message.Body
          };

          await lambda.InvokeAsync(invocation, cancellationToken);
        }

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
      }

      return "OK";
    }

    [LambdaFunction("launch-stepfunction")]
    public async Task<string> LaunchStepFunctionAsync(string input, CancellationToken cancellationToken = default)
    {
      var execution = new StartExecutionRequest
      {
        Name            = Guid.NewGuid().ToString(),
        StateMachineArn = "arn:aws:states:ap-southeast-2:123456789:stateMachine:test-machine",
        Input           = input
      };

      var response = await stepFunctions.StartExecutionAsync(execution, cancellationToken);

      logger.LogInformation($"Started execution: {response.ExecutionArn}");

      return "OK";
    }

    [LambdaFunction("format-message")]
    public string Format(string input) => $"Hello, {input}!";

    [LambdaFunction("capitalize-message")]
    public string Capitalize(string input) => input.ToUpper();

    [LambdaFunction("print-message")]
    public void Print(string input) => logger.LogTrace(input);
  }
}