using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

public interface IConversionService
{
    Task SetConversionAsync(string domain, long shopifyWebhookId, int shopifyOrderId, string code, string landingSite,
        string referralSite, string currency, string orderStatus, decimal orderCost, DateTime shopifyOrderCreated);
    Task UpdateConversionCancelledAsync(string domain, int shopifyOrderId, string orderStatus);
    Task<ConversionDTO?> UpdateConversionFulfilledAsync(string domain, int shopifyOrderId, string orderStatus);
}