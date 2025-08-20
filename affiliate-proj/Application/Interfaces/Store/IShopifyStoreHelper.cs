using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IShopifyStoreHelper
{
    Task<Shop?> GetShopifyStoreInfoAsync(string shop, string accessToken);
    Task<bool> CheckStoreExistsByDomainAsync(string shop);
    Task<Core.Entities.Store?> GetStoreDetailsByShopifyStoreIdAsync(long shopifyStoreId);
    Task SetShopifyStoreAsync(string shop, AuthorizationResult authorizationResult);
    Task<Core.Entities.Store?> UpdateStoreDetailsAsync(Core.Entities.Store store);
}