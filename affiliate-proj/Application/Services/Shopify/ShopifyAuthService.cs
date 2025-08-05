using affiliate_proj.Application.Interfaces.Shopify;
using ShopifySharp;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyAuthService :  IShopifyAuthService
{
    public Task<string> GenerateInstallUrlAsync(string shop)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state)
    {
        throw new NotImplementedException();
    }
}