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
    
    public async Task<CreateStoreDTO?> SetShopifyStoreAsync(CreateStoreDTO storeDto)
    {
        var checkStoreExists = await _postgresDbContext.Stores.FirstOrDefaultAsync(
            store => store.ShopifyId == storeDto.ShopifyId);
        
        if (checkStoreExists != null)
            throw new NullReferenceException("Shopify store already exists");

        var newStore = new Core.Entities.Store
        {
            ShopifyId = storeDto.ShopifyId,
            StoreName = storeDto.StoreName,
            ShopifyToken = storeDto.ShopifyToken,
            StoreUrl = storeDto.StoreUrl,
            ShopifyStoreName = storeDto.ShopifyStoreName,
            ShopifyOwnerName = storeDto.ShopifyOwnerName,
            ShopifyOwnerEmail = storeDto.ShopifyOwnerEmail,
            ShopifyOwnerPhone = storeDto.ShopifyOwnerPhone,
            ShopifyCountry = storeDto.ShopifyCountry,
            ShopifyGrantedScopes = storeDto.ShopifyGrantedScopes,
            UserId = storeDto.UserId,
        };
        
        await _postgresDbContext.Stores.AddAsync(newStore);
        await _postgresDbContext.SaveChangesAsync();
        
        checkStoreExists = await _postgresDbContext.Stores.FirstOrDefaultAsync(
            store => store.ShopifyId == storeDto.ShopifyId);

        CreateStoreDTO returnedStore = new CreateStoreDTO
        {
            ShopifyId = checkStoreExists.ShopifyId,
            StoreName = checkStoreExists.StoreName,
            ShopifyToken = checkStoreExists.ShopifyToken,
            StoreUrl = checkStoreExists.StoreUrl,
            ShopifyStoreName = checkStoreExists.ShopifyStoreName,
            ShopifyOwnerName = checkStoreExists.ShopifyOwnerName,
            ShopifyOwnerEmail = checkStoreExists.ShopifyOwnerEmail,
            ShopifyOwnerPhone = checkStoreExists.ShopifyOwnerPhone,
            ShopifyCountry = checkStoreExists.ShopifyCountry,
            ShopifyGrantedScopes = checkStoreExists.ShopifyGrantedScopes,
            UserId = checkStoreExists.UserId,
        };
        
        return returnedStore;
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

    public async Task<Core.Entities.Store?> UpdateStoreDetailsAsync(Core.Entities.Store store)
    {
        var dbStore = await _postgresDbContext.Stores.FindAsync(store.StoreId);
        if (dbStore == null)
            throw new NullReferenceException("Store not found");
        
        dbStore.ShopifyId = store.ShopifyId;
        dbStore.ShopifyToken = store.ShopifyToken;
        dbStore.StoreUrl = store.StoreUrl;
        dbStore.ShopifyStoreName = store.ShopifyStoreName;
        dbStore.ShopifyOwnerName = store.ShopifyOwnerName;
        dbStore.ShopifyOwnerEmail = store.ShopifyOwnerEmail;
        dbStore.ShopifyOwnerPhone = store.ShopifyOwnerPhone;
        dbStore.ShopifyCountry = store.ShopifyCountry;
        dbStore.ShopifyGrantedScopes = store.ShopifyGrantedScopes;
        
        await _postgresDbContext.SaveChangesAsync();
        return dbStore;
    }
}