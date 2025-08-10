using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using AIContactAssistant.Console.Extensions;

namespace AIContactAssistant.Console.Services;

/// <summary>
/// Contact Assistant Service - handles AI-powered customer interactions
/// </summary>
public class ContactAssistantService : BackgroundService
{
    private readonly Kernel _kernel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ContactAssistantService> _logger;

    public ContactAssistantService(Kernel kernel, IServiceProvider serviceProvider, ILogger<ContactAssistantService> logger)
    {
        _kernel = kernel;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Register plugins with the kernel silently
        _kernel.RegisterAllPlugins(_serviceProvider);
        
        // Execute the sales inquiry scenario
        await ExecuteSalesScenario();
        
        // Exit the application after scenario completion
        Environment.Exit(0);
    }

    private async Task ExecuteSalesScenario()
    {
        try
        {
            // Start the interactive LLM-driven conversation
            var conversationService = new LLMDrivenScenarioService(_kernel, _serviceProvider.GetService<ILogger<LLMDrivenScenarioService>>()!);
            await conversationService.ExecuteLLMDrivenScenario();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
