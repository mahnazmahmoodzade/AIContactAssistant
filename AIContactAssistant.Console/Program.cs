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
        services.AddHostedService<ContactAssistantService>();
    }

    private static void ConfigureSemanticKernel(IServiceCollection services, IConfiguration configuration)
    {
        // Try Azure OpenAI first, fallback to OpenAI
        var azureEndpoint = configuration["AzureOpenAI:Endpoint"];
        var azureApiKey = configuration["AzureOpenAI:ApiKey"];
        var deploymentName = configuration["AzureOpenAI:DeploymentName"];

        if (!string.IsNullOrEmpty(azureEndpoint) && !string.IsNullOrEmpty(azureApiKey))
        {
            // Use Azure OpenAI
            services.AddKernel()
                .AddAzureOpenAIChatCompletion(
                    deploymentName: deploymentName ?? "gpt-4",
                    endpoint: azureEndpoint,
                    apiKey: azureApiKey
                );
        }
        else
        {
            // Fallback to regular OpenAI
            var openAiApiKey = GetApiKey(configuration);
            services.AddKernel()
                .AddOpenAIChatCompletion(
                    modelId: "gpt-4", 
                    apiKey: openAiApiKey
                );
        }
    }

    private static string GetApiKey(IConfiguration configuration)
    {
        return configuration["OpenAI:ApiKey"] ?? 
               Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? 
               "demo-key-replace-with-real-key";
    }

    private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
    {
        // Additional logging configuration can be added here
        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
    }
}
