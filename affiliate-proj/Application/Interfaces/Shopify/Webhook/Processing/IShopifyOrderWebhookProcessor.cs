using affiliate_proj.Core.Enums;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Processing;

public interface IShopifyOrderWebhookProcessor
{
    Task<WebhookOutcomes> HandlePaidOrderAsync();
}