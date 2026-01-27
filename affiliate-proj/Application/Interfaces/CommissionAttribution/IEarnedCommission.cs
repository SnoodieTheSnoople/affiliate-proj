using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Interfaces.CommissionAttribution;

public interface IEarnedCommission
{
    public Task CalculateAttributedCommissionAsync(ConversionDTO conversionDto);
}