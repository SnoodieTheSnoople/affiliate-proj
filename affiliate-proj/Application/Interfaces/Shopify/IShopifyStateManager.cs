namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyStateManager
{
    Task SetStoreStateAsync(string state, Guid userId);
    Task<bool> VerifyStoreStateAsync(string state);
    Task<Guid> GetUserIdFromStateMetadataAsync(string state);
    Task RemoveStoreStateAsync(string state);
}