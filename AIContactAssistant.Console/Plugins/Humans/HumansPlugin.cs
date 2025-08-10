using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Humans
{
    [Description("Human agent escalation service for complex issues that require human expertise, empathy, or specialized knowledge")]
    public class HumansPlugin
    {
        [KernelFunction]
        [Description("Transfers customer conversation to human agent when AI cannot resolve issue or customer requests human assistance. Provides queue status and estimated wait time.")]
        public Task<object> TransferToAgent(
            [Description("Reason for transfer: 'complex_technical_issue', 'complaint', 'sales_negotiation', 'customer_request', 'billing_dispute', 'cancellation'")] string reason) =>
            Task.FromResult<object>(new { 
                status = "queued",
                transferId = "transfer_" + Guid.NewGuid().ToString("N")[..8],
                etaSeconds = 120,
                queuePosition = 3,
                reason = reason,
                priority = reason.Contains("complaint") ? "high" : "normal",
                agentSkills = GetRequiredSkills(reason),
                customerNotified = true
            });

        private static string[] GetRequiredSkills(string reason)
        {
            return reason switch
            {
                var r when r.Contains("technical") => new[] { "technical_support", "network_troubleshooting" },
                var r when r.Contains("sales") => new[] { "sales", "product_knowledge" },
                var r when r.Contains("billing") => new[] { "billing", "account_management" },
                var r when r.Contains("complaint") => new[] { "customer_service", "conflict_resolution" },
                _ => new[] { "general_support" }
            };
        }
    }
}