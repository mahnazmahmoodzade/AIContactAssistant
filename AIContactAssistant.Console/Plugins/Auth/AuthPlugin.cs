using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Auth;

[Description("Authentication and authorization service for user session management and access control")]
public class AuthPlugin
{
    [KernelFunction]
    [Description("Retrieves current authenticated user information from active session. Essential for personalizing service and checking permissions.")]
    public Task<object> GetCurrentUser(
        [Description("Active session identifier from user's authentication token")] string sessionId) =>
        Task.FromResult<object>(new { 
            userId = "u123", 
            roles = new[] { "customer", "verified" }, 
            name = "Anna Novak",
            accountType = "individual",
            authenticationLevel = "strong",
            sessionExpiry = DateTime.Now.AddHours(2)
        });
}