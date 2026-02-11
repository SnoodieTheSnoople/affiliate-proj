using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Processing;
using affiliate_proj.Core.Enums;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Processing;

public class ShopifyOrderWebhookProcessor : IShopifyOrderWebhookProcessor
{
    private readonly IConversionService _conversionService;
    private readonly IEarnedCommissionService _earnedCommissionService;
    private readonly ILogger<ShopifyOrderWebhookProcessor> _logger;
    private readonly PostgresDbContext _dbContext;

    public ShopifyOrderWebhookProcessor(IConversionService conversionService, IEarnedCommissionService earnedCommissionService, ILogger<ShopifyOrderWebhookProcessor> logger, PostgresDbContext dbContext)
    {
        _conversionService = conversionService;
        _earnedCommissionService = earnedCommissionService;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<WebhookOutcomes> HandlePaidOrderAsync(string domain, long shopifyWebhookId, int shopifyOrderId,
        string code, string landingSite, string referralSite, string currency, string orderStatus, decimal orderCost,
        DateTimeOffset shopifyOrderCreated)
    {
        throw new NotImplementedException();
    }
}