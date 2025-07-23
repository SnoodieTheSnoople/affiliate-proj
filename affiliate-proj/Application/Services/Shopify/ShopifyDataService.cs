using affiliate_proj.Application.Interfaces.Shopify;
using ShopifySharp;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyDataService : IShopifyDataService
{
    public Task<IEnumerable<Product>> GetProductsAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetOrdersAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Fulfillment>> GetFulfillmentsAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Refund>> GetRefundsAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<Shop> GetShopAsync(string shopDomain, string accessToken)
    {
        throw new NotImplementedException();
    }
}