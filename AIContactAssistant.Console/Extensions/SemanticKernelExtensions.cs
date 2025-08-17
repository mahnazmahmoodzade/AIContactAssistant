using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace AIContactAssistant.Console.Extensions;

/// <summary>
/// Extension methods for registering Semantic Kernel plugins with dependency injection
/// </summary>
public static class SemanticKernelExtensions
{
    /// <summary>
    /// Registers all AI Contact Assistant plugins with the service collection
    /// </summary>
    public static IServiceCollection AddContactAssistantPlugins(this IServiceCollection services)
    {
        // Automatically discover and register all plugin types
        var pluginTypes = typeof(SemanticKernelExtensions).Assembly
            .GetTypes()
            .Where(t => t.Name.EndsWith("Plugin") && 
                       t.IsClass && 
                       !t.IsAbstract &&
                       t.Namespace?.Contains("Plugins") == true)
            .ToArray();

        foreach (var pluginType in pluginTypes)
        {
            services.AddSingleton(pluginType);
        }

        return services;
    }

    /// <summary>
    /// Registers all plugins with the Semantic Kernel
    /// </summary>
    public static Kernel RegisterAllPlugins(this Kernel kernel, IServiceProvider services)
    {
        // Automatically discover all plugin types from the assembly
        var pluginTypes = typeof(SemanticKernelExtensions).Assembly
            .GetTypes()
            .Where(t => t.Name.EndsWith("Plugin") && 
                       t.IsClass && 
                       !t.IsAbstract &&
                       t.Namespace?.Contains("Plugins") == true)
            .ToArray();

        // Register each plugin using its class name (without "Plugin" suffix)
        foreach (var pluginType in pluginTypes)
        {
            var plugin = services.GetRequiredService(pluginType);
            var pluginName = pluginType.Name.Replace("Plugin", "");
            kernel.Plugins.AddFromObject(plugin, pluginName);
        }

        return kernel;
    }
}
