namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

public interface IConversionService
{
    Task SetConversionAsync(string domain,long shopifyWebhookId, int shopifyOrderId, string code, string landingSite, 
        string referralSite, string currency, string orderStatus, decimal orderCost);
}