using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify.Data.Factories;

public interface IGraphServiceFactory
{
    GraphService CreateGraphService(string shopDomain, string accessToken);
}