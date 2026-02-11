using affiliate_proj.Core.Enums;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Processing;

public interface IShopifyOrderWebhookProcessor
{
    Task<WebhookOutcomes> HandlePaidOrderAsync(string domain, long shopifyWebhookId, int shopifyOrderId, string code, string landingSite,
        string referralSite, string currency, string orderStatus, decimal orderCost, DateTimeOffset shopifyOrderCreated);
}