using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Interfaces.CommissionAttribution;

public interface IEarnedCommissionService
{
    public Task<decimal> CalculateAttributedCommissionAsync(ConversionDTO conversionDto,
        Guid creatorId);
    public Task SetEarnedCommissionAsync(ConversionDTO conversionDto);
}