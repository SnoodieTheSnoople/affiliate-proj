using affiliate_proj.Application.Interfaces.Shopify.Data.Factories;
using ShopifySharp;

namespace affiliate_proj.Application.Services.Shopify.Data.Factories;

public class GraphServiceFactory : IGraphServiceFactory
{
    public GraphService CreateGraphService(string shopDomain, string accessToken)
    {
        throw new NotImplementedException();
    }
}