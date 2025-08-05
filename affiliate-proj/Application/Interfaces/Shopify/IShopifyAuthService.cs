using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyAuthService
{
    Task<string> GenerateInstallUrlAsync(string shop);
    Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state,
        Dictionary<string, string> queryParams);
    
}