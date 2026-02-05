using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Interfaces.CommissionAttribution;

public interface IEarnedCommissionService
{
    public Task CalculateAttributedCommissionAsync(ConversionDTO conversionDto);
    public Task SetEarnedCommissionAsync(ConversionDTO conversionDto);
}