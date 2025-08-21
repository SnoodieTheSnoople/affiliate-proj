using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using ShopifySharp;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookService : IShopifyWebhookService
{
    private readonly IWebhookService _webhookService;
    private readonly IConfiguration _configuration;

    public ShopifyWebhookService(IWebhookService webhookService, IConfiguration configuration)
    {
        _webhookService = webhookService;
        _configuration = configuration;
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