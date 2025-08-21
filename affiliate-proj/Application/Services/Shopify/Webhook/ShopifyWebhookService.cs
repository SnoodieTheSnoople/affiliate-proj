using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using ShopifySharp;
using ShopifySharp.Lists;

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
        
        // Console.WriteLine(_configuration.GetValue<string>("Shopify:BaseUrl"));
        var url = $"{_configuration.GetValue<string>("Shopify:BaseUrl")}/api/webhooks/shopifywebhook/app/uninstalled";
        // Console.WriteLine(url);

        var appUninstalledWebhook = new ShopifySharp.Webhook()
        {
            Address = url,
            CreatedAt = DateTime.Now,
            Topic = "app/uninstalled",
            Format = "json"
        };
        
        await webhookService.CreateAsync(appUninstalledWebhook);
    }
    
    public async Task<ListResult<ShopifySharp.Webhook>> GetAllWebhooksAsync(string shop, string accessToken)
    {
        throw new NotImplementedException();
    }
}