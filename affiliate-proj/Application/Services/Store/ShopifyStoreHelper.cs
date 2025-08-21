using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Store;
using Microsoft.EntityFrameworkCore;
using ShopifySharp;
using ShopifySharp.Utilities;

namespace affiliate_proj.Application.Services.Store;

public class ShopifyStoreHelper : IShopifyStoreHelper
{
    private readonly IShopifyDomainUtility _shopifyDomainUtility;
    private readonly PostgresDbContext _postgresDbContext;

    public ShopifyStoreHelper(IShopifyDomainUtility shopifyDomainUtility, PostgresDbContext postgresDbContext)
    {
        _shopifyDomainUtility = shopifyDomainUtility;
        _postgresDbContext = postgresDbContext;
    }
    public async Task<Shop?> GetShopifyStoreInfoAsync(string shop, string accessToken)
    {
        if (!await ValidateKeyProperties(shop, accessToken))
            return null;
        
        var shopService = new ShopService(shop, accessToken);
        var shopInfo = await shopService.GetAsync();
        
        return shopInfo;
    }

    public async Task<bool> CheckStoreExistsByDomainAsync(string shop)
    {
        var store = await _postgresDbContext.Stores.FirstOrDefaultAsync(s => s.StoreUrl == shop);
        return store != null;
    }
    
    public async Task<Core.Entities.Store> GetStoreByDomainAsync(string shop)
    {
        var store = await _postgresDbContext.Stores.FirstOrDefaultAsync(s => s.StoreUrl == shop);
        return store;
    }
    
    public async Task<Core.Entities.Store?> GetStoreDetailsByShopifyStoreIdAsync(long shopifyStoreId)
    {
        var store = await _postgresDbContext.Stores.FirstOrDefaultAsync(s => s.ShopifyId == shopifyStoreId);
        return store;
    }
    
    public async Task<bool> SetShopifyStoreAsync(string shop, AuthorizationResult authorizationResult, Guid userId)
    {
        var shopDetails = await GetShopifyStoreInfoAsync(shop, authorizationResult.AccessToken);
        var newStoreEntry = new Core.Entities.Store
        {
            ShopifyId = (long) shopDetails.Id,
            ShopifyToken = authorizationResult.AccessToken,
            StoreUrl = shop,
            ShopifyStoreName = shopDetails.Name,
            ShopifyOwnerName = shopDetails.ShopOwner,
            ShopifyOwnerEmail = shopDetails.Email,
            ShopifyOwnerPhone = shopDetails.Phone,
            ShopifyCountry = shopDetails.Country,
            ShopifyGrantedScopes = String.Join(",", authorizationResult.GrantedScopes),
            UserId = userId,
            IsActive = true
        };
        
        await _postgresDbContext.Stores.AddAsync(newStoreEntry);
        await _postgresDbContext.SaveChangesAsync();
        
        var checkStoreExists = await CheckStoreExistsByDomainAsync(shop);
        return checkStoreExists;
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
    
    private async Task<bool> ValidateKeyProperties(string shop, string accessToken)
    {
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        if (string.IsNullOrEmpty(accessToken))
            throw new Exception("Internal Error 007: Invalid access token");
        
        return true;
    }
}