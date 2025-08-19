using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IShopifyStoreHelper
{
    Task<Shop?> GetShopifyStoreInfoAsync(string shop, string accessToken);
    Task<Core.Entities.Store?> GetStoreDetailsByShopifyStoreIdAsync(long shopifyStoreId);
    Task<Core.Entities.Store?> UpdateStoreDetailsAsync(Core.Entities.Store store);
}