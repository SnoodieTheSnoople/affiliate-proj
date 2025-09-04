using affiliate_proj.Core.DataTypes.GraphQL;
using ShopifySharp.Services.Graph;

namespace affiliate_proj.Application.Interfaces.Shopify.Data;

public interface IShopifyProductService
{
    Task<GraphResult<ProductsCountResult>> GetProductsCount(string shopDomain, string accessToken);
    Task<GraphResult<CustomListProductsResult>> GetProductsAsync(string shopDomain, string accessToken, int limit = 250);
    Task SetProductsAsync(string shopDomain, string accessToken);
}