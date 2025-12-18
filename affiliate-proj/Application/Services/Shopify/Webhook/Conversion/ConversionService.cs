using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Conversion;

public class ConversionService : IConversionService
{
    private readonly IShopifyStoreHelper _shopifyStoreHelper;
    private readonly ILogger<ConversionService> _logger;
    private readonly IAffiliateLinkService _affiliateLinkService;
    private readonly IAffiliateCodeService _affiliateCodeService;
    private readonly IConversionRepository _conversionRepository;

    public ConversionService(IShopifyStoreHelper shopifyStoreHelper, ILogger<ConversionService> logger, IAffiliateLinkService affiliateLinkService, IAffiliateCodeService affiliateCodeService, IConversionRepository conversionRepository)
    {
        _shopifyStoreHelper = shopifyStoreHelper;
        _logger = logger;
        _affiliateLinkService = affiliateLinkService;
        _affiliateCodeService = affiliateCodeService;
        _conversionRepository = conversionRepository;
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
        var clicks = 0;
        var orderCreated = DateTime.UtcNow;

        if (!String.IsNullOrEmpty(landingSite))
        {
            // Query DB and get AffiliateLink details
            var affiliateLink = await _affiliateLinkService.GetAffiliateLinkByLinkAsync(landingSite);
            clicks = affiliateLink.Clicks;
        }
        
        var newConversion = new CreateConversion()
        {
            StoreId = store.StoreId,
            Link = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            Clicks =  clicks,
            Code = String.IsNullOrEmpty(code) ? "" : code.Trim(),
            ShopifyOrderId = shopifyOrderId,
            OrderCost = orderCost,
            Currency = currency,
            OrderStatus = orderStatus,
            OrderCreated = orderCreated, // TODO: Check if Webhook send order creation time
            LandingSite = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            LandingSiteRef = String.IsNullOrEmpty(referralSite) ? "" : referralSite,
            Note = code,
        };
        
        // Make call to repository
        await _conversionRepository.SetConversionAsync(newConversion);
        
        // TODO: Identify best fit for conversion tracking to inject into cart_notes/note_attributes
        throw new NotImplementedException();
    }
}