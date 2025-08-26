using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Account;
using ShopifySharp;
using ShopifySharp.Lists;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookService : IShopifyWebhookService
{
    private readonly IConfiguration _configuration;
    private readonly IShopifyStoreHelper  _shopifyStoreHelper;
    private readonly IShopifyWebhookRepository _shopifyWebhookRepository;

    public ShopifyWebhookService(IConfiguration configuration, IShopifyStoreHelper shopifyStoreHelper, IShopifyWebhookRepository shopifyWebhookRepository)
    {
        _configuration = configuration;
        _shopifyStoreHelper = shopifyStoreHelper;
        _shopifyWebhookRepository = shopifyWebhookRepository;
    }

    public async Task RegisterWebhookAsync(string shop, string accessToken)
    {
        var webhookService = new WebhookService(shop, accessToken);
        
        // Console.WriteLine(_configuration.GetValue<string>("Shopify:BaseUrl"));
        var url = $"{_configuration.GetValue<string>("Shopify:BaseUrl")}/api/webhooks/shopifywebhook/app/uninstalled";
        // Console.WriteLine(url);
        
        var listOfWebhooksRegistration = _configuration.GetSection("Shopify:Webhooks").Get<List<string>>();

        // TODO: Change to register a list of webhooks stored in appsettings.json
        var appUninstalledWebhook = new ShopifySharp.Webhook()
        {
            Address = url,
            CreatedAt = DateTime.Now,
            Topic = "app/uninstalled",
            Format = "json"
        };
        
        await webhookService.CreateAsync(appUninstalledWebhook);
        
        // TODO: Move to own method to reduce bloat
        var webhooksEnum = await GetAllWebhooksAsync(shop, accessToken);

        var storeDetails = await _shopifyStoreHelper.GetStoreByDomainAsync(shop);

        using var webhooks = webhooksEnum.Items.GetEnumerator();
        while (webhooks.MoveNext())
        {
            var item = webhooks.Current;
            var webhookEntry = new CreateWebhookRegistrationDTO
            {
                StoreUrl = shop,
                ShopifyWebhookId = (long)item.Id!,
                Topic = item.Topic,
                Format = item.Format,
                RegisteredAt = item.CreatedAt.Value.ToUniversalTime(),
                StoreId = storeDetails.StoreId,
            };

            await _shopifyWebhookRepository.SetShopifyWebhookAsync(webhookEntry);
        }
    }
    
    public async Task RegisterWebhooksAsync(string shop, string accessToken)
    {
        var webhookService = new WebhookService(shop, accessToken);
        
        var baseUrl = _configuration.GetValue<string>("Shopify:BaseUrl");
        
        var listOfWebhooksRegistration = _configuration.GetSection("Shopify:Webhooks").Get<List<string>>();

        foreach (var webhook in listOfWebhooksRegistration)
        {
            var registerWebhook = new ShopifySharp.Webhook()
            {
                Address = $"{baseUrl}/api/webhooks/shopifywebhook/{webhook.Trim()}",
                CreatedAt = DateTime.Now,
                Topic = webhook.Trim(),
                Format = "json"
            };
            
            await webhookService.CreateAsync(registerWebhook);
        }
        
        var registeredWebhooksEnum = await GetAllWebhooksAsync(shop, accessToken);
        await AddAllWebhooksToDb(shop, registeredWebhooksEnum);
    }
    
    public async Task<ListResult<ShopifySharp.Webhook>> GetAllWebhooksAsync(string shop, string accessToken)
    {
        var listOfWebhooksRegistration = _configuration.GetSection("Shopify:Webhooks").Get<List<string>>();
        Console.WriteLine(String.Join(", ", listOfWebhooksRegistration));
        var webhookService = new WebhookService(shop, accessToken);
        var webhooksEnumerable = await webhookService.ListAsync();
        return webhooksEnumerable;
    }

    public async Task UpdateAllWebhookAsync(string shop, string accessToken)
    {
        var webhookService =  new WebhookService(shop, accessToken);
        var baseUrl = _configuration.GetValue<string>("Shopify:BaseUrl");

        var listOfWebhooksRegistered = await GetAllWebhooksAsync(shop, accessToken);
        
        using var webhooks = listOfWebhooksRegistered.Items.GetEnumerator();
        while (webhooks.MoveNext())
        {
            var item = webhooks.Current;
            await webhookService.UpdateAsync((long)item.Id, new ShopifySharp.Webhook()
            {
                Address = $"{baseUrl}/api/webhooks/shopifywebhook/{item.Topic.Trim()}"
            });
        }
    }

    public async Task RemoveWebhookAsync(string shop, string accessToken, long webhookId)
    {
        var webhookService = new WebhookService(shop, accessToken);
        
        var webhookDbReturn = await _shopifyWebhookRepository.DeleteShopifyWebhookAsync(webhookId);
        if (webhookDbReturn)
            await webhookService.DeleteAsync(webhookId);
        
    }

    public async Task RemoveWebhookAsync(Shop shop)
    {
        await _shopifyWebhookRepository.DeleteShopifyWebhooksAsync(shop);
    }

    public async Task RemoveWebhooksAsync(Guid storeId)
    {
        await _shopifyWebhookRepository.DeleteShopifyWebhooksAsync(storeId);
    }

    private async Task AddAllWebhooksToDb(string shop, ListResult<ShopifySharp.Webhook> registeredWebhooksEnum)
    {
        var storeDetails = await _shopifyStoreHelper.GetStoreByDomainAsync(shop);

        using var webhooks = registeredWebhooksEnum.Items.GetEnumerator();
        while (webhooks.MoveNext())
        {
            var item = webhooks.Current;
            var webhookEntry = new CreateWebhookRegistrationDTO
            {
                StoreUrl = shop,
                ShopifyWebhookId = (long)item.Id!,
                Topic = item.Topic,
                Format = item.Format,
                RegisteredAt = item.CreatedAt.Value.ToUniversalTime(),
                StoreId = storeDetails.StoreId,
            };

            await _shopifyWebhookRepository.SetShopifyWebhookAsync(webhookEntry);
        }
    }
}