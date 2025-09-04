using affiliate_proj.Core.DataTypes.GraphQL;
using ShopifySharp.Services.Graph;

namespace affiliate_proj.Application.Interfaces.Shopify.Data;

public interface IShopifyProductService
{
    Task<int> GetProductsCount(string shopDomain, string accessToken);
    Task<GraphResult<CustomListProductsResult>> GetProductsAsync(string shopDomain, string accessToken, int limit = 250);
}