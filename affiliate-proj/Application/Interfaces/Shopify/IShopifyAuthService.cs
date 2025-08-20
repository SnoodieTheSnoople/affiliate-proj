using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyAuthService
{
    Task<string> GenerateInstallUrlAsync(string shop);
    Task<string> GenerateInstallUrlWithUserIdAsync(string shop, Guid userId);
    Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state,
        string queryParams);
}