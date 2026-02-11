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

    public ShopifyOrderWebhookProcessor(IConversionService conversionService, IEarnedCommissionService earnedCommissionService, ILogger<ShopifyOrderWebhookProcessor> logger)
    {
        _conversionService = conversionService;
        _earnedCommissionService = earnedCommissionService;
        _logger = logger;
    }

    public async Task<WebhookOutcomes> HandlePaidOrderAsync()
    {
        throw new NotImplementedException();
    }
}