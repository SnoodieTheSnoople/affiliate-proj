using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using ShopifySharp;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookService : IShopifyWebhookService
{
    private readonly IConfiguration _configuration;

    public ShopifyWebhookService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task RegisterWebhookAsync(string shop, string accessToken)
    {
        var webhookService = new WebhookService(shop, accessToken);

        var appUninstalledWebhook = new ShopifySharp.Webhook()
        {
            Address = $"{_configuration.GetValue<string>("Shopify:BaseUrl")}/api/shopify-webhook/app/uninstalled",
        };
        throw new NotImplementedException();
    }
}