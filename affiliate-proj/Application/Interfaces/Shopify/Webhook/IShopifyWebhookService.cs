namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public class IShopifyWebhookService
{
    Task RegisterWebhookAsync(string shop, string accessToken);
}