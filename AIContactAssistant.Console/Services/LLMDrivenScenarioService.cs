using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Logging;
using AIContactAssistant.Console.Extensions;

namespace AIContactAssistant.Console.Services;

/// <summary>
/// Demonstrates real LLM-driven tool calling using Semantic Kernel's automatic function calling
/// </summary>
public class LLMDrivenScenarioService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly ILogger<LLMDrivenScenarioService> _logger;

    public LLMDrivenScenarioService(Kernel kernel, ILogger<LLMDrivenScenarioService> logger)
    {
        _kernel = kernel;
        _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        _logger = logger;
    }

    /// <summary>
    /// Execute a real interactive console conversation with LLM-driven tool calling
    /// </summary>
    public async Task<object> ExecuteLLMDrivenScenario()
    {
        try
        {
            System.Console.WriteLine("\n🤖 INTERACTIVE AI CONTACT ASSISTANT");
            System.Console.WriteLine(new string('=', 70));
            System.Console.WriteLine("💡 Type your message and press Enter. Type 'quit' or 'exit' to end conversation.");
            System.Console.WriteLine(new string('=', 70));

            // Create chat history
            var chatHistory = new ChatHistory();

            // System prompt to define the AI assistant's role and capabilities
            var systemPrompt = @"You are an expert telecom customer service AI assistant. You have access to various tools to help customers with their telecommunications needs.

Your available tools include:
- Address validation and serviceability checking
- Product catalog search and recommendations  
- Personalized offer creation and order processing
- Identity verification (KYC) and eSIM provisioning
- Notifications and CRM logging
- Privacy protection (PII redaction)

When a customer contacts you:
1. Understand their needs through natural conversation
2. Use appropriate tools to gather information and process requests
3. Provide helpful, accurate responses based on tool outputs
4. Complete the full customer journey from inquiry to activation

Be conversational, helpful, and explain what you're doing at each step. Always acknowledge what tools you're using and why.";

            chatHistory.AddSystemMessage(systemPrompt);

            // Configure OpenAI settings for automatic function calling
            var executionSettings = new OpenAIPromptExecutionSettings
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                MaxTokens = 1500,
                Temperature = 0.3
            };

            var conversationTurns = 0;
            var startTime = DateTime.Now;

            // Interactive conversation loop
            while (true)
            {
                // Get user input
                System.Console.Write("\n👤 YOU: ");
                var userInput = System.Console.ReadLine();

                // Check for exit conditions
                if (string.IsNullOrWhiteSpace(userInput) || 
                    userInput.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                    userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    System.Console.WriteLine("\n👋 Thank you for using the AI Contact Assistant. Goodbye!");
                    break;
                }

                conversationTurns++;
                chatHistory.AddUserMessage(userInput);

                // Show processing indicator
                System.Console.WriteLine($"\n🤖 AI ASSISTANT (analyzing and using tools as needed...)");

                try
                {
                    // Get LLM response with automatic tool calling
                    var response = await _chatCompletionService.GetChatMessageContentAsync(
                        chatHistory, 
                        executionSettings, 
                        _kernel);

                    // Display AI response
                    System.Console.WriteLine($"🤖 AI ASSISTANT: {response.Content}");
                    
                    // Add to chat history
                    chatHistory.AddAssistantMessage(response.Content ?? "");

                    // Show some metadata if available
                    if (response.Metadata?.ContainsKey("Usage") == true)
                    {
                        System.Console.WriteLine($"   💡 Functions were automatically called based on your request");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"❌ Error processing your request: {ex.Message}");
                    _logger.LogError(ex, "Error during conversation turn {Turn}", conversationTurns);
                }

                System.Console.WriteLine(new string('-', 70));
            }

            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            return new
            {
                scenarioStatus = "completed_successfully",
                startTime = startTime,
                endTime = endTime,
                duration = duration.ToString(@"mm\:ss"),
                approach = "Interactive LLM-driven with real-time tool calling",
                toolsUsed = "Determined dynamically by LLM based on conversation",
                conversationTurns = conversationTurns,
                totalMessages = chatHistory.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during interactive conversation");
            System.Console.WriteLine($"❌ Error: {ex.Message}");
            throw;
        }
    }
}
