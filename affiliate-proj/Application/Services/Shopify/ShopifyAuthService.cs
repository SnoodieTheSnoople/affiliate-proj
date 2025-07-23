using affiliate_proj.Application.Interfaces.Shopify;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyAuthService :  IShopifyAuthService
{
    public string BuildAuthUrl(string shopDomain, string state)
    {
        throw new NotImplementedException();
    }

    public Task<string> AuthorizeAsync(string code, string shopDomain)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsValidWebhookAsync(string requestBody, string shopifyHmacHeader)
    {
        throw new NotImplementedException();
    }
}