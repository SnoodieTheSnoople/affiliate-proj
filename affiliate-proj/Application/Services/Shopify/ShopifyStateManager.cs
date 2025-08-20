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
            Expires = DateTime.Now.AddMinutes(10)
        };
        
        _memoryCache.Set($"shopifyOAuthState-{state}", metadata, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });
        return Task.CompletedTask;
    }
    
    public bool VerifyStoreState(string state)
    {
        throw new NotImplementedException();
    }
}