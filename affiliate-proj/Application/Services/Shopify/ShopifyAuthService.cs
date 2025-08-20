using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Store;
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
    private readonly IStoreService _storeService;
    private readonly IShopifyStoreHelper _shopifyStoreHelper;

    public ShopifyAuthService(IConfiguration configuration,
        IShopifyRequestValidationUtility shopifyRequestValidationUtility,
        IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility,
        IMemoryCache memoryCache, ILogger<ShopifyAuthService> logger, PostgresDbContext postgresDbContext,
        IStoreService storeService, IShopifyStoreHelper shopifyStoreHelper)
    {
        _configuration = configuration;
        _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
        _shopifyDomainUtility = shopifyDomainUtility;
        _shopifyOauthUtility = shopifyOauthUtility;
        _memoryCache = memoryCache;
        _logger = logger;
        _postgresDbContext = postgresDbContext;
        _storeService = storeService;
        _shopifyStoreHelper = shopifyStoreHelper;
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
        
        _memoryCache.Set($"ShopifyOAuthState-{state}", state);

        var authUrl = _shopifyOauthUtility.BuildAuthorizationUrl(scopeAsList, shop, 
            clientId, redirectUrl, state);
        
        return authUrl.ToString();
    }

    public async Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state, 
        string queryParams)
    {
        if (await ValidateOAuthProperties(code, shop, state)) 
            _memoryCache.Remove($"ShopifyOAuthState-{state}");
        
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
        
        await UpdateStoreAfterCallback(shop, authorisation);

        return authorisation;
    }

    private async Task UpdateStoreAfterCallback(string shop, AuthorizationResult authorisation)
    {
        var shopDetails = await _shopifyStoreHelper.GetShopifyStoreInfoAsync(shop, authorisation.AccessToken);
        if (shopDetails == null) return;
        _logger.LogInformation($"Obtained shop: {shopDetails.Name} & {shopDetails.Id}");
        
        var checkStoreExists = await _shopifyStoreHelper.GetStoreDetailsByShopifyStoreIdAsync((long) shopDetails.Id);
        _logger.LogInformation($"Is null? {checkStoreExists==null}");

        if (checkStoreExists == null) return;
        
        _logger.LogInformation($"Obtained store: {checkStoreExists.StoreId}");
        var updatedStoreInfo = new Core.Entities.Store
        {
            StoreId = checkStoreExists.StoreId,
            StoreName = checkStoreExists.StoreName,
            ShopifyId = (long) shopDetails.Id,
            ShopifyToken = authorisation.AccessToken,
            StoreUrl = shopDetails.Domain,
            ShopifyStoreName = shopDetails.Name,
            ShopifyOwnerName = shopDetails.ShopOwner,
            ShopifyOwnerEmail = shopDetails.Email,
            ShopifyOwnerPhone = shopDetails.Phone,
            ShopifyCountry = shopDetails.Country,
            ShopifyGrantedScopes = String.Join(",", authorisation.GrantedScopes)
        };
        await _shopifyStoreHelper.UpdateStoreDetailsAsync(updatedStoreInfo);
    }

    private async Task<bool> ValidateOAuthProperties(string code, string shop, string state)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(shop))
            throw new Exception("Internal Error 002: Invalid code or shop");
        
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        var savedState = _memoryCache.Get($"ShopifyOAuthState-{state}").ToString();
        if (String.IsNullOrEmpty(savedState)) 
            throw new Exception("Internal Error 003: No saved state");
        
        Console.WriteLine($"Saved State: {savedState}");
        
        if (!String.Equals(savedState, state)) 
            throw new Exception("Internal Error 004: Invalid state");
        
        return true;
    }
}