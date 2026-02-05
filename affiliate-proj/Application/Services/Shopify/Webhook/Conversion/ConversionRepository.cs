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

    public async Task<ConversionDTO?> GetConversionByIdAsync(Guid conversionId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ConversionDTO>> GetConversionsByStoreIdAsync(Guid storeId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ConversionDTO>> GetConversionsByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ConversionDTO>> GetConversionsByLandingSiteAsync(string landingSite)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateConversionCancelledAsync(Guid storeId, int shopifyOrderId, string orderStatus)
    {
        var conversion = await _dbContext.Conversions.FirstOrDefaultAsync(x =>
            x.StoreId == storeId && x.ShopifyOrderId == shopifyOrderId);

        if (conversion == null)
        {
            return;
        }
        
        conversion.OrderStatus = orderStatus;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ConversionDTO?> UpdateConversionFulfilledAsync(Guid storeId, int shopifyOrderId, string orderStatus)
    {
        var conversion = await _dbContext.Conversions.FirstOrDefaultAsync(x =>
            x.StoreId == storeId && x.ShopifyOrderId == shopifyOrderId);

        if (conversion == null)
        {
            return null;
        }
        
        conversion.OrderStatus = orderStatus;
        await _dbContext.SaveChangesAsync();

        conversion = await _dbContext.Conversions.FirstOrDefaultAsync(x => 
            x.StoreId == storeId && x.ShopifyOrderId == shopifyOrderId);
        
        return ConvertEntityToDto(conversion);
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

    private ConversionDTO ConvertEntityToDto(Core.Entities.Conversion conversion)
    {
        return new ConversionDTO
        {
            ConversionId = conversion.ConversionId,
            StoreId = conversion.StoreId,
            Link = conversion.Link,
            Clicks = conversion.Clicks,
            Code = conversion.Code,
            ShopifyOrderId = conversion.ShopifyOrderId,
            OrderCost = conversion.OrderCost,
            Currency = conversion.Currency,
            OrderStatus = conversion.OrderStatus,
            OrderCreated = conversion.OrderCreated,
            LandingSite = conversion.LandingSite,
            LandingSiteRef = conversion.LandingSiteRef,
            Note = conversion.Note,
            CreatedAt = conversion.CreatedAt,
        };
    }
}