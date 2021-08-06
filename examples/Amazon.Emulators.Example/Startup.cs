using System;
using System.Threading.Tasks;
using Amazon.Emulators.Example.Handlers;
using Amazon.Lambda;
using Amazon.Lambda.Diagnostics;
using Amazon.Lambda.Hosting;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Amazon.SNS;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.StepFunction.Hosting;
using Amazon.StepFunctions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Amazon.Emulators.Example
{
  public sealed class Startup
  {
    public static IHostBuilder HostBuilder => new HostBuilder()
      .ConfigureAppConfiguration((context, builder) =>
      {
        var environment = context.HostingEnvironment.EnvironmentName;

        builder.AddJsonFile(path: "appsettings.json",                optional: true, reloadOnChange: true);
        builder.AddJsonFile(path: $"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        builder.AddEnvironmentVariables();
      })
      .UseStartup<Startup>();

    public static async Task<int> Main(string[] args)
    {
      return await HostBuilder.RunLambdaConsoleAsync(args);
    }

    public Startup(IConfiguration configuration, IHostingEnvironment environment)
    {
      Configuration = configuration;
      Environment   = environment;
    }

    public IConfiguration      Configuration { get; }
    public IHostingEnvironment Environment   { get; }

    [UsedImplicitly]
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddLogging(builder =>
      {
        builder.AddLambdaLogger();
        builder.SetMinimumLevel(Environment.IsDevelopment() ? LogLevel.Trace : LogLevel.Information);
      });

      services.AddFunctionalHandlers<ExampleHandlers>();

      if (Environment.IsDevelopment())
      {
        services.AddEmulator<IAmazonSQS, AmazonSQSEmulator>(
          _ => new AmazonSQSEmulator(
            endpoint: RegionEndpoint.APSoutheast2,
            accountId: 123456789,
            factory: url => new FileSystemQueue(url, basePath: "./Queues")
            {
              VisibilityTimeout = TimeSpan.FromSeconds(5)
            }
          )
        );

        services.AddEmulator<IAmazonS3, AmazonS3Emulator>(
          _ => new AmazonS3Emulator(
            factory: name => new FileSystemBucket(name, basePath: "./Buckets")
          )
        );

        services.AddEmulator<IAmazonSimpleNotificationService, AmazonSNSEmulator>(
          _ => new AmazonSNSEmulator()
        );

        services.AddEmulator<IAmazonLambda, AmazonLambdaEmulator>(
          provider => new AmazonLambdaEmulator(
            resolver: provider.ToLambdaResolver()
          )
        );

        services.AddEmulator<IAmazonStepFunctions, AmazonStepFunctionsEmulator>(
          provider => new AmazonStepFunctionsEmulator(
            resolver: (_, _, _) => EmbeddedResources.TestMachine,
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