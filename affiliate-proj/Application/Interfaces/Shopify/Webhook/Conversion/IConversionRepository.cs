using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;

public interface IConversionRepository
{
    Task SetConversionAsync(CreateConversion createConversion);
}