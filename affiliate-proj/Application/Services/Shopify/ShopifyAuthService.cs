using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Core.Entities.Shopify;
using Microsoft.AspNetCore.Authentication.BearerToken;
using ShopifySharp.Utilities;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyAuthService :  IShopifyAuthService
{
    private readonly ShopifyConfig _shopifyConfig;
    private readonly ILogger<ShopifyAuthService> _logger;
    private readonly HttpClient _httpClient;

    public ShopifyAuthService(ShopifyConfig shopifyConfig, ILogger<ShopifyAuthService> logger,
        HttpClient httpClient)
    {
        _shopifyConfig = shopifyConfig;
        _logger = logger;
        _httpClient = httpClient;
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

    public async Task<string> AuthoriseAsync(string code, string shopDomain)
    {
        try
        {
            var redirectUrl = $"{_shopifyConfig.AppUrl}/auth/callback";
            var formattedShopDomain = FormatShopDomain(shopDomain);

            var tokenRequestData = new Dictionary<string, string>
            {
                { "client_id", _shopifyConfig.ApiKey },
                { "client_secret", _shopifyConfig.ApiSecret },
                { "code", code },
                { "redirect_uri", redirectUrl },
            };

            var tokenRequestContent = new FormUrlEncodedContent(tokenRequestData);
            var tokenUrl = $"https://{formattedShopDomain}/admin/oauth/access_token";

            var response = await _httpClient.PostAsync(tokenUrl, tokenRequestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<AccessTokenResponse>(responseContent);

            if (tokenResponse.AccessToken == null)
                throw new InvalidOperationException("No access token found");
            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ex.Message;
        }
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