using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Services.CommissionAttribution;

public class EarnedCommission : IEarnedCommission
{
    /*
     * 1. Upon order_status fulfilled, calculate the commission based on the predefined rate for the store and creator.
     * 2. Identify return date.
     * 3. Past return date, finalize the commission and mark it as earned.
     * 4. Update the EarnedCommission entity with the finalized amount.
     * 5. Next step to use TotalCommission
     */
    public async Task CalculateAttributedCommissionAsync(ConversionDTO conversionDto)
    {
        throw new NotImplementedException();
    }
}