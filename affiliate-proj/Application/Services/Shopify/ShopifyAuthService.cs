using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Core.Entities.Shopify;
using ShopifySharp.Utilities;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyAuthService :  IShopifyAuthService
{
    private readonly ShopifyConfig _shopifyConfig;
    private readonly ILogger<ShopifyAuthService> _logger;

    public ShopifyAuthService(ShopifyConfig shopifyConfig, ILogger<ShopifyAuthService> logger)
    {
        _shopifyConfig = shopifyConfig;
        _logger = logger;
    }
    public string BuildAuthUrl(string shopDomain, string state)
    {
        var scopes = string.Join(",", _shopifyConfig.PermissionScopes);
        var redirectUrl = $"{_shopifyConfig.AppUrl}/auth/callback";
        
        var formattedShopDomain = FormatShopDomain(shopDomain);

        return null;
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