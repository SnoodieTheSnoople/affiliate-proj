using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Core.DTOs.Shopify.Conversion;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Shopify.Webhook.Conversion;

public class ConversionRepository : IConversionRepository
{
    private readonly PostgresDbContext _dbContext;

    public ConversionRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SetConversionAsync(CreateConversion createConversion)
    {
        var entity = ConvertDtoToEntity(createConversion);
        
        var checkExists = await _dbContext.Conversions.FirstOrDefaultAsync(x => 
            x.ShopifyOrderId == entity.ShopifyOrderId && x.StoreId == entity.StoreId);
        
        if (checkExists != null)
        {
            return;
        }
        
        await _dbContext.Conversions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
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