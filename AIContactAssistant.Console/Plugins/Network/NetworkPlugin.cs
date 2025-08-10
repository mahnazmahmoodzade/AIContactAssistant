using Microsoft.SemanticKernel;

namespace AIContactAssistant.Console.Plugins.Network
{
    public class NetworkPlugin
    {
        [KernelFunction]
        public Task<object> CheckCoverageOrOutage(string address, string postalCode) =>
            Task.FromResult<object>(new { outage = false });
    }
}