using System.Collections;
using affiliate_proj.Core.DataTypes;
using affiliate_proj.Core.DataTypes.GraphQL;
using affiliate_proj.Core.Entities;
using ShopifySharp;
using ShopifySharp.Lists;
using ShopifySharp.Services.Graph;

namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyDataService
{
    Task<IEnumerable<Order>> GetOrdersAsync(string shopDomain, string accessToken, int limit = 250);
    Task<IEnumerable<Fulfillment>> GetFulfillmentsAsync(string shopDomain, string accessToken, int limit = 250);
    Task<IEnumerable<Refund>> GetRefundsAsync(string shopDomain, string accessToken, int limit = 250);
    Task<Shop> GetShopAsync(string shopDomain, string accessToken);
}