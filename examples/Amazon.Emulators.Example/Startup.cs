using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Hosting;
using Amazon.Lambda.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
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

    [LambdaFunction("sqs-test")]
    public async Task QueueTest(IAmazonSQS sqs, IAmazonLambda lambda, CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName, cancellationToken)).QueueUrl;

      for (var i = 0; i < 100; i++)
      {
        await sqs.SendMessageAsync(queueUrl, "Hello, World!", cancellationToken);
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
          var execution = new InvokeRequest
          {
            FunctionName = "lambda-test",
            Payload      = message.Body
          };

          var result = await lambda.InvokeAsync(execution, cancellationToken);

          using (var reader = new StreamReader(result.Payload))
          {
            Console.WriteLine(await reader.ReadToEndAsync());
          }
        }

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
      }
    }

    [LambdaFunction("lambda-test")]
    public Task<string> LambdaTest(string input, CancellationToken cancellationToken = default)
    {
      return Task.FromResult(input.ToUpper());
    }

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
            queueFactory: url => new InMemoryQueue(url)
          )
        );

        services.AddEmulator<IAmazonLambda, AmazonLambdaEmulator>(
          provider => new AmazonLambdaEmulator(
            resolver: (input, context) => provider.ResolveLambdaHandler(input, context).ExecuteAsync
          )
        );
      }
      else
      {
        services.AddSQS();
        services.AddLambda();
      }
    }
  }
}