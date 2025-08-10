using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Catalog
{
    [Description("Personalized offer engine that creates customized pricing and service packages based on customer requirements")]
    public class OffersPlugin
    {
        [KernelFunction]
        [Description("Creates a personalized offer tailored to customer needs and technical constraints. Considers pricing, discounts, bundle opportunities, and service availability.")]
        public Task<object> BuildPersonalizedOffer(
            [Description("Customer needs object containing requirements like: {lines: number, roamingEU: boolean, dataUsage: string, budget: number}")] object needs, 
            [Description("Technical constraints object like: {techs: array, location: string, timeline: string}")] object constraints) =>
            Task.FromResult<object>(new
            {
                offerId = "offer_123",
                productId = "plan_5g_family",
                name = "Family Unlimited 5G EU",
                price = 59.90m,
                currency = "EUR",
                validUntil = DateTime.Now.AddDays(30),
                terms = new
                {
                    lines = 4,
                    data = "unlimited",
                    roaming = "EU included",
                    contractPeriod = 24,
                    setupFee = 0m
                },
                deliveryOptions = new[] { "eSIM", "physical SIM", "home delivery" },
                discountsApplied = new[] { "family_discount_10_percent" },
                reasonForRecommendation = "Perfect match for 4-line family with EU travel needs"
            });

        [KernelFunction]
        [Description("Retrieves detailed information about an existing offer including terms, pricing, and validity period.")]
        public Task<object> GetOffer(
            [Description("Offer identifier to retrieve details for")] string offerId) =>
            Task.FromResult<object>(new
            {
                offerId = offerId,
                status = "active",
                validUntil = DateTime.Now.AddDays(30),
                productId = "plan_5g_family",
                name = "Family Unlimited 5G EU",
                price = 59.90m,
                currency = "EUR"
            });
    }
}