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
        if (!isValidDomain) throw new Exception("Shopify Domain Not Valid");
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

    public Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state)
    {
        throw new NotImplementedException();
    }
}