using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Example.Internal;
using Amazon.Lambda.Core;
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
    public static IHostBuilder HostBuilder => new HostBuilder()
      .UseStartup<Startup>();

    public static async Task<int> Main(string[] args)
      => await HostBuilder.RunLambdaConsoleAsync(args);

    [UsedImplicitly]
    public static async Task<object> ExecuteAsync(object input, ILambdaContext context)
      => await HostBuilder.RunLambdaAsync(input, context);

    [LambdaFunction("producer")]
    public async void Producer(IAmazonSQS sqs, CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync("test-queue", cancellationToken)).QueueUrl;

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
    public async void Consumer(IAmazonSQS sqs, CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync("test-queue", cancellationToken)).QueueUrl;

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
      services.AddFunctionalHandlers<Startup>();

      if (environment.IsDevelopment())
      {
        services.ReplaceWithEmbedded<IAmazonSQS, EmbeddedSQS>();
      }
    }
  }
}