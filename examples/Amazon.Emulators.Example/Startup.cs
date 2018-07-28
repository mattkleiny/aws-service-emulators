﻿using System;
using System.Threading.Tasks;
using Amazon.Emulators.Example.Handlers;
using Amazon.Lambda;
using Amazon.Lambda.Diagnostics;
using Amazon.Lambda.Hosting;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.StepFunction;
using Amazon.StepFunctions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Amazon.Emulators.Example
{
  public sealed class Startup
  {
    public static IHostBuilder HostBuilder
      => new HostBuilder().UseStartup<Startup>();

    public static async Task<int> Main(string[] args)
      => await HostBuilder.RunLambdaConsoleAsync(args);

    [UsedImplicitly]
    public void ConfigureServices(IServiceCollection services, IHostingEnvironment environment)
    {
      services.AddLogging(builder =>
      {
        builder.AddLambdaLogger();
        builder.SetMinimumLevel(environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information);
      });

      services.AddFunctionalHandlers<ExampleHandlers>();

      if (environment.IsDevelopment())
      {
        services.AddEmulator<IAmazonSQS, AmazonSQSEmulator>(
          provider => new AmazonSQSEmulator(
            endpoint: RegionEndpoint.APSoutheast2,
            accountId: 123456789,
            factory: url => new InMemoryQueue(url)
          )
        );

        services.AddEmulator<IAmazonS3, AmazonS3Emulator>(
          provider => new AmazonS3Emulator(
            factory: name => new FileSystemBucket(name, basePath: "./Buckets")
          )
        );

        services.AddEmulator<IAmazonLambda, AmazonLambdaEmulator>(
          provider => new AmazonLambdaEmulator(
            resolver: provider.ToLambdaResolver()
          )
        );

        services.AddEmulator<IAmazonStepFunctions, AmazonStepFunctionsEmulator>(
          provider => new AmazonStepFunctionsEmulator(
            resolver: (region, id, name) => EmbeddedResources.TestMachine,
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
        services.AddS3();
        services.AddLambda();
        services.AddStepFunctions();
      }
    }
  }
}