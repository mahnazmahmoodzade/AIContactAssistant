using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Catalog
{
    [Description("Product catalog service for searching and retrieving telecommunications plans, devices, and services")]
    public class CatalogPlugin
    {
        [KernelFunction]
        [Description("Searches the product catalog for plans and services that match customer requirements. Use natural language queries like 'family plan 4 lines EU roaming' or technical filters.")]
        public Task<object> SearchProducts(
            [Description("Natural language search query describing customer needs (e.g., 'family plan with unlimited data')")] string query, 
            [Description("Optional filters object with properties like: techs (array), priceRange, features, etc.")] object filters) =>
            Task.FromResult<object>(new
            {
                products = new[]
                {
                    new
                    {
                        productId = "plan_5g_family",
                        name = "Family Unlimited 5G EU",
                        price = 59.90m,
                        currency = "EUR",
                        lines = 4,
                        data = "unlimited",
                        roaming = new[] { "EU" },
                        technologies = new[] { "5G", "FTTH" },
                        features = new[] { "EU roaming", "Unlimited data", "4 lines" },
                        matchReasons = new[] { "4 lines requested", "EU roaming included", "Unlimited data" }
                    }
                },
                matchScore = 0.95,
                totalResults = 1
            });

        [KernelFunction]
        [Description("Retrieves comprehensive details about a specific product by its ID. Use this for detailed specifications and pricing information.")]
        public Task<object> GetProduct(
            [Description("Product identifier from search results")] string productId) =>
            Task.FromResult<object>(new
            {
                id = productId,
                name = "Family Unlimited 5G EU",
                price = 59.90m,
                currency = "EUR",
                description = "Perfect family plan with 4 lines, unlimited data, and EU roaming",
                specifications = new
                {
                    lines = 4,
                    data = "unlimited",
                    roaming = new[] { "EU", "Switzerland", "Norway" },
                    speed = "up to 1 Gbps",
                    networkTech = "5G/LTE",
                    contractPeriod = 24,
                    setupFee = 0
                },
                eligibility = new
                {
                    minAge = 18,
                    creditCheck = true,
                    kycRequired = true
                }
            });
    }
}