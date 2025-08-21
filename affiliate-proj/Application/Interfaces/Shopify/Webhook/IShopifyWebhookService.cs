using ShopifySharp.Lists;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookService
{
    Task RegisterWebhookAsync(string shop, string accessToken);
    Task<ListResult<ShopifySharp.Webhook>> GetAllWebhooksAsync(string shop, string accessToken);
}