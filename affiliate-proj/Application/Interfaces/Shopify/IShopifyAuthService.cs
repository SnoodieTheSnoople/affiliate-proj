using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify;

public interface IShopifyAuthService
{
    Task<string> GenerateInstallUrlAsync(string shop);
    Task<AuthorizationResult> HandleCallbackAsync(string code, string shop, string state,
        string queryParams);
    Task<Shop?> GetShopifyStoreIdAsync(string shopDomain, string accessToken);
    Task<Core.Entities.Store?> SetShopifyStoreAsync(Shop shopDetails, AuthorizationResult authorizationResult);
    Task<Core.Entities.Store?> SetShopifyStoreAsync(Core.Entities.Store storeDto);
}