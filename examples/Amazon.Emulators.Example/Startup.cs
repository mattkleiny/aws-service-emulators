using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Hosting;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.StepFunction;
using Amazon.StepFunctions;
using Amazon.StepFunctions.Model;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Amazon.Emulators.Example
{
  public sealed class Startup
  {
    private const string QueueName = "test-queue";

    public static IHostBuilder HostBuilder
      => new HostBuilder().UseStartup<Startup>();

    public static async Task<int> Main(string[] args)
      => await HostBuilder.RunLambdaConsoleAsync(args);

    [LambdaFunction("emulator-test")]
    public async Task EmulatorTest(IAmazonSQS sqs, IAmazonStepFunctions stepFunctions, CancellationToken cancellationToken = default)
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

        foreach (var message in batch.Messages)
        {
          var execution = new StartExecutionRequest
          {
            Name            = Guid.NewGuid().ToString(),
            StateMachineArn = "arn:aws:states:ap-southeast-2:123456789:stateMachine:test-machine",
            Input           = message.Body
          };

          var response = await stepFunctions.StartExecutionAsync(execution, cancellationToken);

          Console.WriteLine($"Started execution: {response.ExecutionArn}");
        }

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
      }
    }

    [LambdaFunction("format-message")]
    public string Format(string input) => $"Hello, {input}!";

    [LambdaFunction("capitalize-message")]
    public string Capitalize(string input) => input.ToUpper();

    [LambdaFunction("print-message")]
    public void Print(string input) => Console.WriteLine(input);

    [UsedImplicitly]
    public void ConfigureServices(IServiceCollection services, IHostingEnvironment environment)
    {
      services.AddFunctionalHandlers<Startup>();

      if (environment.IsDevelopment())
      {
        services.AddEmulator<IAmazonSQS, AmazonSQSEmulator>(
          provider => new AmazonSQSEmulator(
            endpoint: RegionEndpoint.APSoutheast2,
            accountId: 123456789,
            factory: url => new InterProcessQueue(
              channelName: "emulators-example",
              queueUrl: url
            )
          )
        );

        services.AddEmulator<IAmazonLambda, AmazonLambdaEmulator>(
          provider => new AmazonLambdaEmulator(
            resolver: provider.ToLambdaResolver()
          )
        );

        services.AddEmulator<IAmazonStepFunctions, AmazonStepFunctionsEmulator>(
          provider => new AmazonStepFunctionsEmulator(
            arn =>
            {
              switch (arn.StateMachineName.ToLower())
              {
                case "test-machine": return EmbeddedResources.TestMachine;

                default:
                  throw new Exception($"An unrecognized step function was requested: {arn.StateMachineName}");
              }
            },
            factory: provider.ToStepHandlerFactory(),
            impositions: new Impositions
            {
              WaitTimeOverride = TimeSpan.FromMilliseconds(0)
            }
          )
        );
      }
      else
      {
        services.AddSQS();
        services.AddLambda();
        services.AddStepFunctions();
      }
    }
  }
}