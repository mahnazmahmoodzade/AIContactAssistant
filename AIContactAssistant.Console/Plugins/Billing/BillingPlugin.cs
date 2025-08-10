using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Billing
{
    [Description("Billing and invoice management system for customer account charges, payments, and financial transactions")]
    public class BillingPlugin
    {
        [KernelFunction]
        [Description("Retrieves customer invoices and billing details for a specific account and billing period. Shows itemized charges and total amounts.")]
        public Task<object> GetInvoices(
            [Description("Customer account identifier to retrieve invoices for")] string accountId, 
            [Description("Billing month in format 'YYYY-MM' (e.g., '2025-08')")] string month) =>
            Task.FromResult<object>(new
            {
                total = 89.90,
                currency = "EUR",
                billingPeriod = month,
                status = "paid",
                items = new[] { 
                    new { description = "Base Plan", amount = 49.90, category = "monthly_fee" }, 
                    new { description = "Roaming", amount = 40.00, category = "usage_charges" } 
                },
                paymentMethod = "auto_debit",
                dueDate = "2025-08-15"
            });

        [KernelFunction]
        [Description("Sets up a payment intent for processing customer payments. Handles different payment methods and scheduling.")]
        public Task<object> SetPaymentIntent(
            [Description("Customer account ID for payment processing")] string accountId, 
            [Description("Payment amount in account currency")] double amount, 
            [Description("Payment method: 'credit_card', 'bank_transfer', 'auto_debit', 'paypal'")] string method, 
            [Description("Payment due date in ISO format (YYYY-MM-DD)")] string dueDate) =>
            Task.FromResult<object>(new { 
                status = "scheduled",
                paymentId = "pay_" + Guid.NewGuid().ToString("N")[..8],
                amount = amount,
                scheduledDate = dueDate,
                method = method,
                confirmationSent = true
            });
    }
}