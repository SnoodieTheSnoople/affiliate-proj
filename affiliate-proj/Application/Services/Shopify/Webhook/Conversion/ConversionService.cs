using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Conversion;

public class ConversionService : IConversionService
{
    public async Task SetConversionAsync(string webhookId, string shopifyOrderId, string code, string landingSite, 
        string referralSite, string currency, string orderStatus, int orderCost)
    {
        throw new NotImplementedException();
    }
}