namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyAuthService
{
    string BuildAuthUrl(string shopDomain, string state);
    Task<string> AuthorizeAsync(string code, string shopDomain);
    Task<bool> IsValidWebhookAsync(string requestBody,  string shopifyHmacHeader);
}