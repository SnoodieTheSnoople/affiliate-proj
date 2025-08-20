namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyStateManager
{
    Task SetStoreStateAsync(string state, Guid userId);
    bool VerifyStoreState(string state);
    Guid GetUserIdFromStateMetadata(string state);
}