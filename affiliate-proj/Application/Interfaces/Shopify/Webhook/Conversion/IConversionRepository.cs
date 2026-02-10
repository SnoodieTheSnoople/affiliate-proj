using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

public interface IConversionRepository
{
    Task<ConversionDTO> SetConversionAsync(CreateConversion createConversion);
    Task<ConversionDTO?> GetConversionByIdAsync(Guid conversionId);
    Task<List<ConversionDTO>> GetConversionsByStoreIdAsync(Guid storeId);
    Task<List<ConversionDTO>> GetConversionsByCodeAsync(string code);
    Task<List<ConversionDTO>> GetConversionsByLandingSiteAsync(string landingSite);
    Task UpdateConversionCancelledAsync(Guid storeId, int shopifyOrderId, string orderStatus);
    Task<ConversionDTO?> UpdateConversionFulfilledAsync(Guid storeId, int shopifyOrderId, string orderStatus);
}