using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShopifySharp;
using ShopifySharp.Utilities;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyAuthService :  IShopifyAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IShopifyRequestValidationUtility _shopifyRequestValidationUtility;
    private readonly IShopifyDomainUtility _shopifyDomainUtility;
    private readonly IShopifyOauthUtility _shopifyOauthUtility;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ShopifyAuthService> _logger;
    private readonly PostgresDbContext _postgresDbContext;

    public ShopifyAuthService(IConfiguration configuration,
        IShopifyRequestValidationUtility shopifyRequestValidationUtility,
        IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility,
        IMemoryCache memoryCache, ILogger<ShopifyAuthService> logger, PostgresDbContext postgresDbContext)
    {
        _configuration = configuration;
        _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
        _shopifyDomainUtility = shopifyDomainUtility;
        _shopifyOauthUtility = shopifyOauthUtility;
        _memoryCache = memoryCache;
        _logger = logger;
        _postgresDbContext = postgresDbContext;
    }
    public async Task<string> GenerateInstallUrlAsync(string shop)
    {
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if (!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        _logger.LogInformation("Validated shop domain");
        
        var configScopes = _configuration.GetValue<string>("Shopify:Scopes");
        var clientId = _configuration.GetValue<string>("Shopify:ClientId");
        var redirectUrl =  _configuration.GetValue<string>("Shopify:RedirectUrl");
        _logger.LogInformation($"Scopes: {configScopes}\nClientId: {clientId}\nRedirectUrl: {redirectUrl}");
        
        var scopeAsList = configScopes.Split(",").ToList();
        
        var state = Guid.NewGuid().ToString();
        
        _memoryCache.Set("ShopifyOAuthState", state);

        var authUrl = _shopifyOauthUtility.BuildAuthorizationUrl(scopeAsList, shop, 
            clientId, redirectUrl, state);
        
        return authUrl.ToString();
    }

    public async Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state, 
        string queryParams)
    {
        if (await ValidateOAuthProperties(code, shop, state)) 
            _memoryCache.Remove("ShopifyOAuthState");
        
        var clientId = _configuration.GetValue<string>("Shopify:ClientId");
        var apiSecret = _configuration.GetValue<string>("Shopify:ApiSecret");
        
        var isValidRequest = _shopifyRequestValidationUtility.IsAuthenticRequest(
            queryParams, apiSecret);

        if (!isValidRequest)
            throw new Exception("Internal Error 005: Invalid request. Unable to validate HMAC.");
        
        var authorisation = await _shopifyOauthUtility.AuthorizeAsync(code, shop, clientId, apiSecret);
        
        if (string.IsNullOrEmpty(authorisation.AccessToken))
            throw new Exception("Internal Error 006: Invalid access token");
        
        _logger.LogInformation($"Obtained access_token: {authorisation.AccessToken}");
        
        return authorisation;
    }

    public async Task<Shop?> GetShopifyStoreIdAsync(string shop, string accessToken)
    {
        if (!await ValidateKeyProperties(shop, accessToken))
            return null;
        
        var shopService = new ShopService(shop, accessToken);
        var shopInfo = await shopService.GetAsync();
        
        // var propertyList = typeof(Shop).GetProperties().ToList();
        // foreach (var property in propertyList)
        // {
        //     var value = property.GetValue(shopInfo);
        //     Console.WriteLine($"{property} - {value}");
        // }
        
        return shopInfo;
    }

    public async Task<Store?> SetShopifyStoreAsync(Shop shopDetails, AuthorizationResult authorizationResult)
    {
        try
        {
            var authToken =  authorizationResult.AccessToken;
            if (authToken == null)
                throw new Exception("Internal Error 007: Invalid access token");
            
            var checkStoreExists = await _postgresDbContext.Stores.FirstOrDefaultAsync(
                store => store.ShopifyToken == authToken);
            if (checkStoreExists != null)
                throw new NullReferenceException("Shopify store already exists");

            var newStore = new Store
            {
                ShopifyId = (long)shopDetails.Id,
                ShopifyToken = authToken,
                StoreUrl = shopDetails.Domain,
                ShopifyStoreName = shopDetails.Name,
                ShopifyOwnerName = shopDetails.ShopOwner,
                ShopifyOwnerEmail = shopDetails.Email,
                ShopifyOwnerPhone = shopDetails.Phone,
                ShopifyCountry = shopDetails.Country,
                ShopifyGrantedScopes = String.Join(",", authorizationResult.GrantedScopes),
            };
            
            await _postgresDbContext.Stores.AddAsync(newStore);
            await _postgresDbContext.SaveChangesAsync();
            
            newStore = await _postgresDbContext.Stores.FirstOrDefaultAsync(store => store.ShopifyToken == authToken);
            return newStore;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<Store?> SetShopifyStoreAsync(StoreDTO storeDto)
    {
        var checkStoreExists = await _postgresDbContext.Stores.FirstOrDefaultAsync(
            store => store.ShopifyId == storeDto.ShopifyId);
        
        if (checkStoreExists != null)
            throw new NullReferenceException("Shopify store already exists");

        var newStore = new Store
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
        
        return checkStoreExists;
    }

    private async Task<bool> ValidateOAuthProperties(string code, string shop, string state)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(shop))
            throw new Exception("Internal Error 002: Invalid code or shop");
        
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        var savedState = _memoryCache.Get("ShopifyOAuthState").ToString();
        if (String.IsNullOrEmpty(savedState)) 
            throw new Exception("Internal Error 003: No saved state");
        
        Console.WriteLine($"Saved State: {savedState}");
        
        if (!String.Equals(savedState, state)) 
            throw new Exception("Internal Error 004: Invalid state");
        
        return true;
    }
    
    private async Task<bool> ValidateOAuthProperties(string shop, string state)
    {
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        var savedState = _memoryCache.Get("ShopifyOAuthState").ToString();
        if (String.IsNullOrEmpty(savedState)) 
            throw new Exception("Internal Error 003: No saved state");
        
        Console.WriteLine($"Saved State: {savedState}");
        
        if (!String.Equals(savedState, state)) 
            throw new Exception("Internal Error 004: Invalid state");
        
        return true;
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
    
    private async Task<bool> ValidateKeyProperties(string shop)
    {
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        
        return true;
    }
}