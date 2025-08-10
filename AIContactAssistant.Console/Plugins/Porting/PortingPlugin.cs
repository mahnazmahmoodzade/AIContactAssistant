using Microsoft.SemanticKernel;

namespace AIContactAssistant.Console.Plugins.Porting
{
    public class PortingPlugin
    {
        [KernelFunction]
        public Task<object> CheckEligibility(string msisdn, string currentCarrier) =>
            Task.FromResult<object>(new { eligible = true });

        [KernelFunction]
        public Task<object> Submit(string msisdn, string currentCarrier, string accountPin) =>
            Task.FromResult<object>(new { portDate = "2025-08-10" });
    }
}