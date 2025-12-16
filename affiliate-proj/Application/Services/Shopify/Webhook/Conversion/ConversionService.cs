using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Application.Interfaces.Store;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Conversion;

public class ConversionService : IConversionService
{
    private readonly IShopifyStoreHelper _shopifyStoreHelper;
    private readonly ILogger<ConversionService> _logger;

    public ConversionService(IShopifyStoreHelper shopifyStoreHelper, ILogger<ConversionService> logger)
    {
        _shopifyStoreHelper = shopifyStoreHelper;
        _logger = logger;
    }

    public async Task SetConversionAsync(string domain, long shopifyWebhookId, int shopifyOrderId, string code,
        string landingSite,
        string referralSite, string currency, string orderStatus, decimal orderCost)
    {
        // Query DB to find StoreId using StoreUrl
        // Check if Link, Code, LandingSite, LandingSiteRef are null.
        // TODO: Current assumption. CODE IS NOTE
        // TODO: Find out LandingSiteRef (referring_site) output

        var store = await _shopifyStoreHelper.GetStoreByDomainAsync(domain);

        if (!String.IsNullOrEmpty(landingSite))
        {
            // Query DB and get AffiliateLink details
        }
        
        var newConversion = new Core.Entities.Conversion
        {
            StoreId = store.StoreId,
            Link = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            // If link is available then retrieve clicks
            Code = String.IsNullOrEmpty(code) ? "" : code,
            ShopifyOrderId = shopifyOrderId,
            OrderCost = orderCost,
            Currency = currency,
            OrderStatus = orderStatus,
            OrderCreated = DateTime.UtcNow,
            LandingSite = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            LandingSiteRef = String.IsNullOrEmpty(referralSite) ? "" : referralSite,
            Note = code,
        };
        throw new NotImplementedException();
    }
}