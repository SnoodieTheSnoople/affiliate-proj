using affiliate_proj.Core.DataTypes.Store.Shopify;
using Microsoft.Extensions.Caching.Memory;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyStateManager
{
    private readonly IMemoryCache _memoryCache;

    public ShopifyStateManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task SetStoreStateAsync(string state, Guid userId)
    {
        var metadata = new OauthStateMetadata
        {
            State = state,
            UserId = userId,
        };
        
        _memoryCache.Set($"shopifyOAuthState-{state}", metadata, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return Task.CompletedTask;
    }
    
    public bool VerifyStoreState(string state)
    {
        var savedMetadata = _memoryCache.Get<OauthStateMetadata>($"shopifyOAuthState-{state}");
        if (savedMetadata == null || String.IsNullOrEmpty(savedMetadata.State))
            throw new Exception("Internal Error 003: No saved state");
        
        if (!String.Equals(state, savedMetadata.State))
            throw new Exception("Internal Error 004: Invalid state");

        return true;
    }
    
    public Guid GetUserIdFromStateMetadata(string state)
    {
        var savedMetadata = _memoryCache.Get<OauthStateMetadata>($"shopifyOAuthState-{state}");
        if (savedMetadata == null)
            throw new Exception("Internal Error 004: No saved state");
        
        if (Guid.Empty ==  savedMetadata.UserId)
            throw new Exception("Internal Error 005: Invalid state, userId empty");
        
        return savedMetadata.UserId;
    }
}