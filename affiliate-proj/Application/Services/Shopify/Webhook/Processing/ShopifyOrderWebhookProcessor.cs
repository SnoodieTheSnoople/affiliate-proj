using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Processing;
using affiliate_proj.Core.DataTypes.Records;
using affiliate_proj.Core.Enums;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Processing;

public class ShopifyOrderWebhookProcessor : IShopifyOrderWebhookProcessor
{
    private readonly IConversionService _conversionService;
    private readonly IEarnedCommissionService _earnedCommissionService;
    private readonly ILogger<ShopifyOrderWebhookProcessor> _logger;
    private readonly PostgresDbContext _dbContext;
    
    private readonly IAffiliateLinkService _affiliateLinkService;
    private readonly IAffiliateCodeService _affiliateCodeService;

    public ShopifyOrderWebhookProcessor(IConversionService conversionService, 
        IEarnedCommissionService earnedCommissionService, ILogger<ShopifyOrderWebhookProcessor> logger, 
        PostgresDbContext dbContext, IAffiliateLinkService affiliateLinkService, 
        IAffiliateCodeService affiliateCodeService)
    {
        _conversionService = conversionService;
        _earnedCommissionService = earnedCommissionService;
        _logger = logger;
        _dbContext = dbContext;
        _affiliateLinkService = affiliateLinkService;
        _affiliateCodeService = affiliateCodeService;
    }

    public async Task<WebhookOutcomes> HandlePaidOrderAsync(string domain, long shopifyWebhookId, int shopifyOrderId,
        string code, string landingSite, string referralSite, string currency, string orderStatus, decimal orderCost,
        DateTimeOffset shopifyOrderCreated)
    {
        /*
         * 1. Use dbContext and begin transaction.
         * 2. Call Conversion method first ( CREATE NEW METHOD TO RETURN WebhookOutcomes )
         * 3. Early return for other events that are not successful.
         * 4. Call EarnedCommission ( CREATE NEW METHOD TO RETURN WebhookOutcomes )
         * 5. Commit transaction if all successful, else Rollback.
         */

        await using var transaction = _dbContext.Database.BeginTransaction();

        var conversionResult = await _conversionService.StageSetConversionAsync(domain, shopifyWebhookId, shopifyOrderId
            , code, landingSite, referralSite, currency, orderStatus, orderCost, shopifyOrderCreated);

        if (conversionResult.Status == ConversionStagingStatus.Ignored)
            return WebhookOutcomes.Ignored;
        
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        
        return WebhookOutcomes.ProcessedSuccessfully;
        
        throw new NotImplementedException();
    }
}