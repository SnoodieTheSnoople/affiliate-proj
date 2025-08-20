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
        throw new NotImplementedException();
    }
}