using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using AIContactAssistant.Console.Plugins.Auth;
using AIContactAssistant.Console.Plugins.CRM;
using AIContactAssistant.Console.Plugins.Billing;
using AIContactAssistant.Console.Plugins.Addressing;
using AIContactAssistant.Console.Plugins.Network;
using AIContactAssistant.Console.Plugins.Catalog;
using AIContactAssistant.Console.Plugins.Orders;
using AIContactAssistant.Console.Plugins.Porting;
using AIContactAssistant.Console.Plugins.Appointments;
using AIContactAssistant.Console.Plugins.Humans;
using AIContactAssistant.Console.Plugins.Notifications;

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
        // Register all plugins as singletons
        services.AddSingleton<AuthPlugin>();
        services.AddSingleton<GuardrailsPlugin>();
        services.AddSingleton<KYCPlugin>();
        services.AddSingleton<RedactionPlugin>();
        services.AddSingleton<CRMPlugin>();
        services.AddSingleton<BillingPlugin>();
        services.AddSingleton<UsagePlugin>();
        services.AddSingleton<AddressPlugin>();
        services.AddSingleton<ServiceabilityPlugin>();
        services.AddSingleton<NetworkPlugin>();
        services.AddSingleton<DevicePlugin>();
        services.AddSingleton<CatalogPlugin>();
        services.AddSingleton<OffersPlugin>();
        services.AddSingleton<OrdersPlugin>();
        services.AddSingleton<ProvisioningPlugin>();
        services.AddSingleton<PortingPlugin>();
        services.AddSingleton<AppointmentsPlugin>();
        services.AddSingleton<HumansPlugin>();
        services.AddSingleton<NotificationsPlugin>();

        return services;
    }

    /// <summary>
    /// Registers all plugins with the Semantic Kernel
    /// </summary>
    public static Kernel RegisterAllPlugins(this Kernel kernel, IServiceProvider services)
    {
        // Define plugin registrations
        var pluginRegistrations = new Dictionary<string, Type>
        {
            { "Auth", typeof(AuthPlugin) },
            { "Guardrails", typeof(GuardrailsPlugin) },
            { "KYC", typeof(KYCPlugin) },
            { "Redaction", typeof(RedactionPlugin) },
            { "CRM", typeof(CRMPlugin) },
            { "Billing", typeof(BillingPlugin) },
            { "Usage", typeof(UsagePlugin) },
            { "Address", typeof(AddressPlugin) },
            { "Serviceability", typeof(ServiceabilityPlugin) },
            { "Network", typeof(NetworkPlugin) },
            { "Device", typeof(DevicePlugin) },
            { "Catalog", typeof(CatalogPlugin) },
            { "Offers", typeof(OffersPlugin) },
            { "Orders", typeof(OrdersPlugin) },
            { "Provisioning", typeof(ProvisioningPlugin) },
            { "Porting", typeof(PortingPlugin) },
            { "Appointments", typeof(AppointmentsPlugin) },
            { "Humans", typeof(HumansPlugin) },
            { "Notifications", typeof(NotificationsPlugin) }
        };

        // Register each plugin with the kernel
        foreach (var (name, type) in pluginRegistrations)
        {
            var plugin = services.GetRequiredService(type);
            kernel.Plugins.AddFromObject(plugin, name);
        }

        return kernel;
    }
}
