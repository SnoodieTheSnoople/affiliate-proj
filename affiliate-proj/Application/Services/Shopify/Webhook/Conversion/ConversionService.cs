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
        string referralSite, string currency, string orderStatus, decimal orderCost, DateTime shopifyOrderCreated)
    {
        // Query DB to find StoreId using StoreUrl
        // Check if Link, Code, LandingSite, LandingSiteRef are null.
        // TODO: Current assumption. CODE IS NOTE
        // TODO: Find out LandingSiteRef (referring_site) output

        var store = await _shopifyStoreHelper.GetStoreByDomainAsync(domain);
        var clicks = 0;

        if (!String.IsNullOrEmpty(landingSite))
        {
            // Query DB and get AffiliateLink details
            var affiliateLink = await _affiliateLinkService.GetAffiliateLinkByLinkAsync(landingSite);
            clicks = affiliateLink.Clicks;
        }

        if (!String.IsNullOrEmpty(code))
        {
            // Query DB and get AffiliateCode details
            var affiliateCode = await _affiliateCodeService.GetAffiliateCodeByCodeAsync(code);

            // If no affiliate code found, early return and do not add record
            if (affiliateCode == null)
                return;
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
            OrderCreated = shopifyOrderCreated, // TODO: Check if Webhook send order creation time
            LandingSite = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            LandingSiteRef = String.IsNullOrEmpty(referralSite) ? "" : referralSite,
            Note = code,
        };
        
        // Make call to repository
        await _conversionRepository.SetConversionAsync(newConversion);
        
        // TODO: Identify best fit for conversion tracking to inject into cart_notes/note_attributes
    }

    /*
     * "Cancelled" order status refers to "refunded" orders in Shopify if payment has been made.
     */
    public async Task UpdateConversionCancelledAsync(string domain, int shopifyOrderId, string orderStatus)
    {
        if (String.IsNullOrEmpty(domain))
        {
            _logger.LogWarning("Domain is null or empty in UpdateConversionCancelledAsync");
            return;
        }

        if (String.IsNullOrEmpty(orderStatus))
        {
            _logger.LogWarning("OrderStatus is null or empty in UpdateConversionCancelledAsync");
            return;
        }
        
        var store = await _shopifyStoreHelper.GetStoreByDomainAsync(domain);
        
        await _conversionRepository.UpdateConversionCancelledAsync(store.StoreId, shopifyOrderId, orderStatus);
    }

    // Can reuse Cancellation method. Separate concerns in case of future different handling.
    public async Task UpdateConversionFulfilledAsync(string domain, int shopifyOrderId, string orderStatus)
    {
        if (String.IsNullOrEmpty(domain))
        {
            _logger.LogWarning("Domain is null or empty in UpdateConversionCancelledAsync");
            return;
        }

        if (String.IsNullOrEmpty(orderStatus))
        {
            _logger.LogWarning("OrderStatus is null or empty in UpdateConversionCancelledAsync");
            return;
        }
        
        var store = await _shopifyStoreHelper.GetStoreByDomainAsync(domain);
    }
}