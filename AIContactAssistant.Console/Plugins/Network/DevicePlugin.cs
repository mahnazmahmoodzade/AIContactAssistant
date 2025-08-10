using Microsoft.SemanticKernel;

namespace AIContactAssistant.Console.Plugins.Network
{
    public class DevicePlugin
    {
        [KernelFunction]
        public Task<object> RunDiagnostics(string serviceId) =>
            Task.FromResult<object>(new { status = "weak_signal" });
    }
}