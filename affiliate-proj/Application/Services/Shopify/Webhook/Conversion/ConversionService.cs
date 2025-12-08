using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Conversion;

public class ConversionService : IConversionService
{
    public async Task SetConversionAsync(string shopifyWebhookId, string shopifyOrderId, string code, string landingSite, 
        string referralSite, string currency, string orderStatus, int orderCost)
    {
        var newConversion = new Core.Entities.Conversion
        {
            // Find StoreId
            Link = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            // If link is available then retrieve clicks
            Code = String.IsNullOrEmpty(code) ? "" : code,
            ShopifyOrderId = shopifyOrderId,
            OrderCost = orderCost,
            Currency = currency,
            OrderStatus = orderStatus,
            OrderCreated = DateTime.UtcNow,
            LandingSite = String.IsNullOrEmpty(landingSite) ? "" : landingSite,
            LandingSiteRef = String.IsNullOrEmpty(referralSite) ? "" : referralSite,
            Note = code,
        };
        throw new NotImplementedException();
    }
}