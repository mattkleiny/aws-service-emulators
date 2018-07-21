﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Emulators.Example.Internal;
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
    public async Task Consumer(IAmazonSQS sqs, IAmazonLambda lambda, CancellationToken cancellationToken = default)
    {
      var queueUrl = (await sqs.GetQueueUrlAsync(QueueName, cancellationToken)).QueueUrl;

      await sqs.SendMessageAsync(queueUrl, "Hello, World!");
      await sqs.SendMessageAsync(queueUrl, "Hello, World!");
      await sqs.SendMessageAsync(queueUrl, "Hello, World!");
      await sqs.SendMessageAsync(queueUrl, "Hello, World!");
      await sqs.SendMessageAsync(queueUrl, "Hello, World!");
      await sqs.SendMessageAsync(queueUrl, "Hello, World!");
      await sqs.SendMessageAsync(queueUrl, "Hello, World!");

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
            FunctionName = "handler",
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

    [LambdaFunction("handler")]
    public Task<string> Handler(string input, CancellationToken cancellationToken = default)
    {
      return Task.FromResult(input.ToUpper());
    }

    [UsedImplicitly]
    public void ConfigureServices(IServiceCollection services, IHostingEnvironment environment)
    {
      services.AddFunctionalHandlers<Startup>();

      services.AddSQS();
      services.AddLambda();

      if (environment.IsDevelopment())
      {
        services.ReplaceWithEmbedded<IAmazonSQS, EmbeddedAmazonSQS>();
        services.ReplaceWithEmbedded<IAmazonLambda, EmbeddedAmazonLambda>(provider =>
        {
          var host = provider.GetRequiredService<IHost>();

          return new EmbeddedAmazonLambda(resolver: (input, context) =>
          {
            var handler = host.Services.ResolveLambdaHandler(input, context);
            
            return handler.ExecuteAsync;
          });
        });
      }
    }
  }
}