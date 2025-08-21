using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using ShopifySharp;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookService : IShopifyWebhookService
{
    private readonly IWebhookService _webhookService;

    public ShopifyWebhookService(IWebhookService webhookService)
    {
        _webhookService = webhookService;
    }

    public Task RegisterWebhookAsync(string shop, string accessToken)
    {
        var webhookService = new WebhookService(shop, accessToken);

        var appUninstalledWebhook = new ShopifySharp.Webhook()
        {
            
        };
        throw new NotImplementedException();
    }
}