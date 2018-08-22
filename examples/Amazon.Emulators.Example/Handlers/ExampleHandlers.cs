using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Hosting;
using Amazon.Lambda.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using Microsoft.Extensions.Logging;

namespace Amazon.Emulators.Example.Handlers
{
  public sealed class ExampleHandlers
  {
    private const string QueueName   = "test-queue";
    private const string TestMachine = "arn:aws:states:ap-southeast-2:123456789:stateMachine:test-machine";

    private readonly ILogger<ExampleHandlers> logger;

    private readonly IAmazonSQS           sqs;
    private readonly IAmazonS3            s3;
    private readonly IAmazonLambda        lambda;
    private readonly IAmazonStepFunctions stepFunctions;

    public ExampleHandlers(ILogger<ExampleHandlers> logger, IAmazonSQS sqs, IAmazonS3 s3, IAmazonLambda lambda, IAmazonStepFunctions stepFunctions)
    {
      Check.NotNull(logger, nameof(logger));
      Check.NotNull(sqs, nameof(sqs));
      Check.NotNull(s3, nameof(s3));
      Check.NotNull(lambda, nameof(lambda));
      Check.NotNull(stepFunctions, nameof(stepFunctions));

      this.logger        = logger;
      this.sqs           = sqs;
      this.s3            = s3;
      this.lambda        = lambda;
      this.stepFunctions = stepFunctions;
    }

    [LambdaFunction("entry-point")]
    public async Task<string> EntryPointAsync()
    {
      var request = new PutObjectRequest
      {
        BucketName  = "payloads",
        Key         = "input.txt",
        ContentBody = "test1,test2,test3,test4,test5,test6,test7,test8,test9,test10"
      };

      logger.LogTrace("Writing payload to S3");

      await s3.PutObjectAsync(request);

      logger.LogTrace("Reading payload from S3");

      var payload = await s3.GetObjectAsync(bucketName: "payloads", key: "input.txt");

      using (var reader = new StreamReader(payload.ResponseStream))
      {
        logger.LogTrace("Invoking SQS producer");

        await lambda.InvokeAsync(new InvokeRequest
        {
          FunctionName = "producer",
          Payload      = await reader.ReadToEndAsync()
        });

        logger.LogTrace("Invoking SQS consumer");

        await lambda.InvokeAsync(new InvokeRequest
        {
          FunctionName = "consumer"
        });
      }

      return "OK";
    }

    [LambdaFunction("producer")]
    public async Task<string> ProducerAsync(string input)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName)).QueueUrl;

      foreach (var message in input.Split(','))
      {
        logger.LogTrace($"Sending message '{message}' to SQS at {queueUrl}");

        await sqs.SendMessageAsync(queueUrl, message);
      }

      return "OK";
    }

    [LambdaFunction("consumer")]
    public async Task<string> ConsumerAsync()
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName)).QueueUrl;

      logger.LogTrace("Running SQS consumer");

      while (true)
      {
        var request = new ReceiveMessageRequest
        {
          QueueUrl            = queueUrl,
          MaxNumberOfMessages = 10,
          WaitTimeSeconds     = 5
        };

        var batch = await sqs.ReceiveMessageAsync(request);

        foreach (var message in batch.Messages)
        {
          logger.LogTrace($"Invoking step function lambda with body '{message.Body}'");

          await lambda.InvokeAsync(new InvokeRequest
          {
            FunctionName = "launch-stepfunction",
            Payload      = message.Body
          });

          await sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
      }
    }

    [LambdaFunction("launch-stepfunction")]
    public async Task<string> LaunchStepFunctionAsync(string input)
    {
      var execution = new StartExecutionRequest
      {
        Name            = Guid.NewGuid().ToString(),
        StateMachineArn = TestMachine,
        Input           = input
      };

      logger.LogTrace($"Starting execution with name {execution.Name}");

      var response = await stepFunctions.StartExecutionAsync(execution);

      logger.LogTrace($"Started execution: {response.ExecutionArn}");

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