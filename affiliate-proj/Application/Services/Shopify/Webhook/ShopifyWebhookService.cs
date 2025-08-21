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

    public async Task RegisterWebhookAsync(string shop, string accessToken)
    {
        var webhookService = new WebhookService(shop, accessToken);

        var appUninstalledWebhook = new ShopifySharp.Webhook()
        {
            Address = $"{_configuration.GetValue<string>("Shopify:BaseUrl")}/api/webhooks/shopifywebhook/app/uninstalled",
            CreatedAt = DateTime.Now,
            Topic = "app/uninstalled",
            Format = "json"
        };
        
        await webhookService.CreateAsync(appUninstalledWebhook);
    }

    public async Task<List<ShopifySharp.Webhook>> GetAllWebhooksAsync(string shop, string accessToken)
    {
        throw new NotImplementedException();
    }
}