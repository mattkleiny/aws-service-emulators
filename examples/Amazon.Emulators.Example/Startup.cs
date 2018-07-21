using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Example.Internal;
using Amazon.Lambda;
using Amazon.Lambda.Hosting;
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

    public static IHostBuilder HostBuilder => new HostBuilder()
      .UseStartup<Startup>();

    public static async Task<int> Main(string[] args)
      => await HostBuilder.RunLambdaConsoleAsync(args);

    [LambdaFunction("producer")]
    public async Task Producer(IAmazonSQS sqs, CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName, cancellationToken)).QueueUrl;

      while (!cancellationToken.IsCancellationRequested)
      {
        var request = new SendMessageRequest
        {
          QueueUrl    = queueUrl,
          MessageBody = "Hello, World!"
        };

        await sqs.SendMessageAsync(request, cancellationToken);

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
      }
    }

    [LambdaFunction("consumer")]
    public async Task Consumer(IAmazonSQS sqs, CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName, cancellationToken)).QueueUrl;

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
          Console.WriteLine(message);
        }

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
      }
    }

    [UsedImplicitly]
    public void ConfigureServices(IServiceCollection services, IHostingEnvironment environment)
    {
      services.AddSQS();
      services.AddLambda();
      services.AddFunctionalHandlers<Startup>();

      if (environment.IsDevelopment())
      {
        services.ReplaceWithEmbedded<IAmazonSQS, EmbeddedAmazonSQS>();
        services.ReplaceWithEmbedded<IAmazonLambda, EmbeddedAmazonLambda>();
      }
    }
  }
}