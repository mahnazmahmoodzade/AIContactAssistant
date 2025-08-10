using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Billing
{
    [Description("Usage analytics and consumption tracking for voice, data, SMS, and roaming services across customer accounts")]
    public class UsagePlugin
    {
        [KernelFunction]
        [Description("Provides detailed breakdown of customer usage across all services including data consumption, voice minutes, SMS, and roaming charges for billing period.")]
        public Task<object> GetBreakdown(
            [Description("Customer account identifier to analyze usage for")] string accountId, 
            [Description("Usage period in format 'YYYY-MM' or 'current' for current month")] string period) =>
            Task.FromResult<object>(new { 
                dataGB = 12.5,
                roamingGB = 3.0,
                voiceMinutes = 450,
                smsCount = 89,
                period = period,
                costBreakdown = new {
                    data = 25.50,
                    roaming = 15.75,
                    voice = 8.90,
                    sms = 3.40
                },
                allowances = new {
                    dataRemaining = "unlimited",
                    voiceRemaining = 550,
                    smsRemaining = 211
                }
            });
    }
}