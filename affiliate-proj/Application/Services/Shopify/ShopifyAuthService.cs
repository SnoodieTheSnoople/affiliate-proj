using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Core.Entities.Shopify;
using ShopifySharp.Utilities;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyAuthService :  IShopifyAuthService
{
    private readonly ShopifyConfig _shopifyConfig;
    private readonly ILogger<ShopifyAuthService> _logger;
    private readonly ShopifyDomainUtility _shopifyDomainUtility;

    public ShopifyAuthService(ShopifyConfig shopifyConfig, ILogger<ShopifyAuthService> logger,
        ShopifyDomainUtility shopifyDomainUtility)
    {
        _shopifyConfig = shopifyConfig;
        _logger = logger;
        _shopifyDomainUtility = shopifyDomainUtility;
    }
    public string BuildAuthUrl(string shopDomain, string state)
    {
        var scopes = string.Join(",", _shopifyConfig.PermissionScopes);
        var redirectUrl = $"{_shopifyConfig.AppUrl}/auth/callback";
        
        var formattedShopDomain = FormatShopDomain(shopDomain);

        var authUrl = $"https://{formattedShopDomain}/admin/oauth/authorize?" +
                      $"client_id={_shopifyConfig.ApiKey}&" +
                      $"scope={Uri.EscapeDataString(scopes)}&" +
                      $"redirect_uri={Uri.EscapeDataString(redirectUrl)}&" +
                      $"state={state}";

        return authUrl;
    }

    public Task<string> AuthorizeAsync(string code, string shopDomain)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsValidWebhookAsync(string requestBody, string shopifyHmacHeader)
    {
        throw new NotImplementedException();
    }

    private static string FormatShopDomain(string shopDomain)
    {
        shopDomain = shopDomain.Replace("https://", "").Replace("http://", "");

        if (!shopDomain.EndsWith(".myshopify.com"))
        {
            shopDomain += ".myshopify.com";
        }
        
        return shopDomain;
    }
}