using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Logging;
using AIContactAssistant.Console.Extensions;

namespace AIContactAssistant.Console.Services;

/// <summary>
/// Conversation service that handles interactive customer conversations using AI-powered assistance
/// </summary>
public class ConversationService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly ILogger<ConversationService> _logger;

    public ConversationService(Kernel kernel, ILogger<ConversationService> logger)
    {
        _kernel = kernel;
        _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        _logger = logger;
    }

    /// <summary>
    /// Execute an interactive customer service conversation with AI-powered assistance
    /// </summary>
    public async Task<object> StartCustomerConversation()
    {
        try
        {
            System.Console.WriteLine("\n🤖 INTERACTIVE AI CONTACT ASSISTANT");
            System.Console.WriteLine(new string('=', 70));
            System.Console.WriteLine("💡 Type your message and press Enter. Type 'quit' or 'exit' to end conversation.");
            System.Console.WriteLine(new string('=', 70));

            // Create chat history
            var chatHistory = new ChatHistory();

            // System prompt to define the AI assistant's role and personality (not function descriptions)
            var systemPrompt = @"You are a friendly and professional telecom customer service representative. 

Your personality:
- Helpful and patient with customers
- Professional but conversational tone
- Proactive in identifying customer needs
- Always explain what you're doing and why

Customer service guidelines:
- Greet customers warmly
- Listen to their needs carefully
- Use available tools to help them efficiently
- Provide clear explanations of services and pricing
- Complete the full customer journey from inquiry to resolution
- Always confirm customer satisfaction before ending

You have access to various business tools that will help you assist customers effectively.";

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
                sessionStatus = "completed_successfully",
                startTime = startTime,
                endTime = endTime,
                duration = duration.ToString(@"mm\:ss"),
                serviceType = "Interactive customer service with AI assistance",
                toolsUsed = "Determined dynamically by AI based on customer needs",
                conversationTurns = conversationTurns,
                totalMessages = chatHistory.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error during customer conversation");
            System.Console.WriteLine($"❌ Error: {ex.Message}");
            throw;
        }
    }
}
