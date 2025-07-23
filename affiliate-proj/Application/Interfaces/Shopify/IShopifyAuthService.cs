namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyAuthService
{
    string BuildAuthUrl(string shopDomain, string state);
    Task<string> AuthoriseAsync(string code, string shopDomain);
    Task<bool> IsValidWebhookAsync(string requestBody,  string shopifyHmacHeader);
}