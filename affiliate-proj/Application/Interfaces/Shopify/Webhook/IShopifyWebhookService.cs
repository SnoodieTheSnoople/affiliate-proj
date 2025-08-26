using ShopifySharp;
using ShopifySharp.Lists;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookService
{
    Task RegisterWebhookAsync(string shop, string accessToken, string newTopic);
    Task RegisterWebhooksAsync(string shop, string accessToken);
    Task<ListResult<ShopifySharp.Webhook>> GetAllWebhooksAsync(string shop, string accessToken);
    Task UpdateAllWebhooksAsync(string shop, string accessToken);
    Task RemoveWebhookAsync(string shop, string accessToken, long webhookId);
    Task RemoveWebhookAsync(Shop shop);
    Task RemoveWebhooksAsync(Guid storeId);
}