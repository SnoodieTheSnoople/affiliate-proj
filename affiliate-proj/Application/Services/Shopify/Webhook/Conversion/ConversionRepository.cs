using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Conversion;

public class ConversionRepository : IConversionRepository
{
    public async Task SetConversionAsync(CreateConversion createConversion)
    {
        var entity = ConvertDtoToEntity(createConversion);
        
        throw new NotImplementedException();
    }

    private Core.Entities.Conversion ConvertDtoToEntity(CreateConversion createConversion)
    {
        return new Core.Entities.Conversion
        {
            StoreId = createConversion.StoreId,
            Link = createConversion.Link,
            Clicks = createConversion.Clicks,
            Code = createConversion.Code,
            ShopifyOrderId = createConversion.ShopifyOrderId,
            OrderCost = createConversion.OrderCost,
            Currency = createConversion.Currency,
            OrderStatus = createConversion.OrderStatus,
            OrderCreated = createConversion.OrderCreated,
            LandingSite = createConversion.LandingSite,
            LandingSiteRef = createConversion.LandingSiteRef,
            Note = createConversion.Note,
        };
    }
}