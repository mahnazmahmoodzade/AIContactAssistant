using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using AIContactAssistant.Console.Extensions;
using AIContactAssistant.Console.Services;

namespace AIContactAssistant.Console;

/// <summary>
/// Main program entry point for the AI Contact Assistant
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHost(args);
        
        try
        {
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            var logger = host.Services.GetService<ILogger<Program>>();
            logger?.LogCritical(ex, "Application terminated unexpectedly");
            throw;
        }
    }

    private static IHost CreateHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            .ConfigureLogging(ConfigureLogging)
            .Build();
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var configuration = context.Configuration;
        
        // Register all Contact Assistant plugins
        services.AddContactAssistantPlugins();

        // Configure Semantic Kernel
        ConfigureSemanticKernel(services, configuration);

        // Register application services
        services.AddHostedService<ApplicationHostService>();
    }

    private static void ConfigureSemanticKernel(IServiceCollection services, IConfiguration configuration)
    {
        // Configure Azure OpenAI
        var azureEndpoint = configuration["AzureOpenAI:Endpoint"];
        var azureApiKey = configuration["AzureOpenAI:ApiKey"];
        var deploymentName = configuration["AzureOpenAI:DeploymentName"];

        if (string.IsNullOrEmpty(azureEndpoint) || string.IsNullOrEmpty(azureApiKey))
        {
            throw new InvalidOperationException(
                "Azure OpenAI configuration is required. Please set AzureOpenAI:Endpoint and AzureOpenAI:ApiKey in appsettings.json");
        }

        services.AddKernel()
            .AddAzureOpenAIChatCompletion(
                deploymentName: deploymentName ?? "gpt-4.1",
                endpoint: azureEndpoint,
                apiKey: azureApiKey
            );
    }

    private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
    {
        // Additional logging configuration can be added here
        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
    }
}
