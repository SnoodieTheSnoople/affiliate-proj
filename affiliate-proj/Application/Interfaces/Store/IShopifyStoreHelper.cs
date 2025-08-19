using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IShopifyStoreHelper
{
    Task<Shop?> GetShopifyStoreInfoAsync(string shop, string accessToken);
}