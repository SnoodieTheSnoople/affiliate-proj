using ShopifySharp;
using ShopifySharp.Lists;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookService
{
    Task RegisterWebhookAsync(string shop, string accessToken);
    Task<ListResult<ShopifySharp.Webhook>> GetAllWebhooksAsync(string shop, string accessToken);
    Task UpdateAllWebhookAsync(string shop, string accessToken, long webhookId);
    Task RemoveWebhookAsync(string shop, string accessToken, long webhookId);
    Task RemoveWebhookAsync(Shop shop);
}