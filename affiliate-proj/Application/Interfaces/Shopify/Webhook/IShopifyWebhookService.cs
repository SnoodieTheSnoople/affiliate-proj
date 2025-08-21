namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookService
{
    Task RegisterWebhookAsync(string shop, string accessToken);
}