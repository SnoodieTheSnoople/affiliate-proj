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

    public async Task<Core.Entities.Store?> GetStoreByIdAsync(Guid storeId)
    {
        var returnedStore = await _postgresDbContext.Stores.FindAsync(storeId);
        return returnedStore;
        throw new NotImplementedException();
    }
}