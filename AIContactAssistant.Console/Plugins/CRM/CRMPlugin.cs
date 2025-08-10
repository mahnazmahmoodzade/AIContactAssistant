using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.CRM
{
    [Description("Customer Relationship Management system for account information, interaction tracking, and customer lifecycle management")]
    public class CRMPlugin
    {
        [KernelFunction]
        [Description("Retrieves comprehensive customer account information including services, billing status, contact details, and service history. Essential for personalized service.")]
        public Task<object> GetAccount(
            [Description("Lookup criteria object (e.g., {email: 'user@example.com'} or {phone: '+43123456789'} or {accountId: 'acct_001'})")] object lookup, 
            [Description("Array of information sections to include: ['services', 'billing', 'history', 'preferences', 'contacts']")] string[] include) =>
            Task.FromResult<object>(new
            {
                accountId = "acct_001",
                balance = 49.90,
                status = "active",
                tier = "premium",
                services = new[] { "FTTH", "5G Mobile" },
                customerInfo = new
                { 
                    name = "John Doe",
                    email = "user@example.com",
                    phone = "+43 1 234 5678",
                    address = "Rennweg 22, 1030 Wien"
                },
                accountSummary = new
                {
                    memberSince = "2023-01-15",
                    totalSpent = 1247.50,
                    supportTickets = 3,
                    satisfaction = 4.8
                }
            });

        [KernelFunction]
        [Description("Records customer interaction details in CRM for tracking service quality, sales opportunities, and follow-up requirements. Essential for customer journey management.")]
        public Task<object> LogInteraction(
            [Description("Customer account ID if existing customer, null for prospects")] string? accountId, 
            [Description("Interaction intent: 'sales_inquiry', 'technical_support', 'billing_question', 'order_status', 'complaint'")] string intent, 
            [Description("Reference to conversation transcript or interaction record")] string transcriptRef) =>
            Task.FromResult<object>(new
            {
                interactionId = "int_" + Guid.NewGuid().ToString("N")[..8],
                accountId = accountId ?? "prospect_" + Guid.NewGuid().ToString("N")[..8],
                intent = intent,
                transcriptRef = transcriptRef,
                timestamp = DateTime.Now,
                channel = "ai_assistant",
                status = "logged",
                followUpRequired = intent.Contains("sales") || intent.Contains("order"),
                tags = new[] { "sales", "esim", "family_plan", "new_customer" },
                nextAction = intent.Contains("sales") ? "sales_follow_up" : "none"
            });

        [KernelFunction]
        [Description("Creates new prospect record for potential customers showing interest but not yet converted. Initiates sales pipeline tracking.")]
        public Task<object> CreateProspect(
            [Description("Contact information object with fields like name, email, phone, address")] object contactInfo, 
            [Description("Lead source: 'website', 'ai_assistant', 'referral', 'advertising', 'walk_in'")] string source) =>
            Task.FromResult<object>(new
            {
                prospectId = "pros_" + Guid.NewGuid().ToString("N")[..8],
                status = "new_lead",
                source = source,
                createdAt = DateTime.Now,
                assignedTo = "sales_team",
                priority = "high",
                expectedValue = 719.40, // 12 months * 59.90
                followUpDate = DateTime.Now.AddDays(1)
            });

        [KernelFunction]
        [Description("Updates existing customer record with new information such as contact details, preferences, or service interests.")]
        public Task<object> UpdateCustomer(
            [Description("Customer ID to update")] string customerId, 
            [Description("Update data object containing fields to modify")] object updateData) =>
            Task.FromResult<object>(new
            {
                customerId = customerId,
                status = "updated",
                lastModified = DateTime.Now,
                changes = new[] { "contact_info", "preferences", "service_interest" },
                auditTrail = "Changes logged for compliance"
            });
    }
}