using affiliate_proj.Application.Interfaces.Shopify;
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

    public ShopifyAuthService(IConfiguration configuration,
        IShopifyRequestValidationUtility shopifyRequestValidationUtility,
        IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility,
        IMemoryCache memoryCache, ILogger<ShopifyAuthService> logger)
    {
        _configuration = configuration;
        _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
        _shopifyDomainUtility = shopifyDomainUtility;
        _shopifyOauthUtility = shopifyOauthUtility;
        _memoryCache = memoryCache;
        _logger = logger;
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

    public async Task<Shop> GetShopifyStoreId(string shop, string accessToken)
    {
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        if (string.IsNullOrEmpty(accessToken))
            throw new Exception("Internal Error 007: Invalid access token");
        
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