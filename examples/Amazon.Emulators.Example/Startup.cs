using System;
using System.Threading;
using Amazon.Emulators.Embedded;
using Amazon.Emulators.Example.Internal;
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
      }
    }

    [UsedImplicitly]
    public void ConfigureServices(IServiceCollection services, IHostingEnvironment environment)
    {
      services.AddSQS();
      services.AddFunctionalHandlers<Startup>();

      services.ConfigureHostingOptions(options =>
      {
        if (environment.IsDevelopment())
        {
          services.ReplaceWith<IAmazonSQS, EmbeddedAmazonSQS>();
        }
      });
    }
  }
}