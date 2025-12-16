namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

public interface IConversionService
{
    Task SetConversionAsync(string domain,string shopifyWebhookId, string shopifyOrderId, string code, string landingSite, 
        string referralSite, string currency, string orderStatus, int orderCost);
}