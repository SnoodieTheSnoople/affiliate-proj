using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Account;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Store;

public class StoreService : IStoreService
{
    private readonly PostgresDbContext _postgresDbContext;

    public StoreService(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }
    
    public async Task<List<StoreDTO>> GetAllStoresAsync()
    {
        var stores = await _postgresDbContext.Stores.Select(s => new StoreDTO
        {
            StoreId = s.StoreId,
            StoreName = s.StoreName,
            CreatedAt = s.CreatedAt,
            ShopifyCountry = s.ShopifyCountry,
            StoreUrl = s.StoreUrl,
        }).ToListAsync();
        
        return stores;
    }

    public async Task<StoreDTO> GetStoreByIdAsync(Guid storeId)
    {
        var store = await _postgresDbContext.Stores.FindAsync(storeId);
        if (store == null)
            throw new NullReferenceException("Store not found");

        var returnedStore = new StoreDTO
        {
            StoreId = store.StoreId,
            StoreName = store.StoreName,
            CreatedAt = store.CreatedAt,
            ShopifyCountry = store.ShopifyCountry,
            StoreUrl = store.StoreUrl,
        };
        
        return returnedStore;
    }

    public async Task<Core.Entities.Store> GetStoreDetailsByIdAsync(Guid storeId)
    {
        var store = await _postgresDbContext.Stores.FindAsync(storeId);
        if (store == null)
            throw new NullReferenceException("Store not found");

        return store;
    }

    // Used internally and not exposed to endpoints.
    public async Task<Core.Entities.Store?> GetStoreDetailsByShopifyStoreIdAsync(long shopifyStoreId)
    {
        var store = await _postgresDbContext.Stores.FirstOrDefaultAsync(s => s.ShopifyId == shopifyStoreId);
        return store;
    }
}