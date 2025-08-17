using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using AIContactAssistant.Console.Extensions;

namespace AIContactAssistant.Console.Services;

/// <summary>
/// Application host service - manages application lifecycle, plugin registration, and startup orchestration
/// </summary>
public class ApplicationHostService : BackgroundService
{
    private readonly Kernel _kernel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationHostService> _logger;

    public ApplicationHostService(Kernel kernel, IServiceProvider serviceProvider, ILogger<ApplicationHostService> logger)
    {
        _kernel = kernel;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Register plugins with the kernel silently
        _kernel.RegisterAllPlugins(_serviceProvider);
        
        // Execute the customer conversation workflow
        await ExecuteCustomerWorkflow();
        
        // Exit the application after scenario completion
        Environment.Exit(0);
    }

    private async Task ExecuteCustomerWorkflow()
    {
        try
        {
            // Start the interactive customer conversation
            var conversationService = new ConversationService(_kernel, _serviceProvider.GetService<ILogger<ConversationService>>()!);
            await conversationService.StartCustomerConversation();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
