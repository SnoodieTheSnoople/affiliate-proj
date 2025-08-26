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
    private readonly ILogger<ShopifyWebhookService> _logger;

    public ShopifyWebhookService(IConfiguration configuration, IShopifyStoreHelper shopifyStoreHelper, 
        IShopifyWebhookRepository shopifyWebhookRepository, ILogger<ShopifyWebhookService> logger)
    {
        _configuration = configuration;
        _shopifyStoreHelper = shopifyStoreHelper;
        _shopifyWebhookRepository = shopifyWebhookRepository;
        _logger = logger;
    }

    public async Task RegisterWebhookAsync(string shop, string accessToken, string newTopic)
    {
        var webhookService = new WebhookService(shop, accessToken);
        var url = $"{_configuration.GetValue<string>("Shopify:BaseUrl")}/api/webhooks/shopifywebhook/{newTopic.Trim()}";
        
        var addWebhook = new ShopifySharp.Webhook()
        {
            Address = url,
            CreatedAt = DateTime.Now,
            Topic = newTopic.Trim(),
            Format = "json"
        };
        
        await webhookService.CreateAsync(addWebhook);
        var registeredWebhooksEnum = await GetAllWebhooksAsync(shop, accessToken);
        await AddAllWebhooksToDb(shop, registeredWebhooksEnum);
    }
    
    public async Task RegisterWebhooksAsync(string shop, string accessToken)
    {
        var webhookService = new WebhookService(shop, accessToken);
        var baseUrl = _configuration.GetValue<string>("Shopify:BaseUrl");
        var listOfWebhooksRegistration = _configuration.GetSection("Shopify:Webhooks").Get<List<string>>();

        foreach (var webhook in listOfWebhooksRegistration)
        {
            try
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
            catch (Exception e)
            {
                // Pass, continue to iterate.
                _logger.LogError(e.Message);
            }
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

    public async Task UpdateAllWebhooksAsync(string shop, string accessToken)
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