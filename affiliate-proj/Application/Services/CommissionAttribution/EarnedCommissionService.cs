using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Core.DTOs.Shopify.Conversion;

namespace affiliate_proj.Application.Services.CommissionAttribution;

public class EarnedCommissionService : IEarnedCommissionService
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
        /*
         * 1. Lookup affiliate_code or landing_site/landing_site_ref to identify the creator. Fetch CreatorId.
         * 2. Lookup CommissionRates based on StoreId and CreatorId to get the commission rate.
         * 3. Calculate commission: AmtEarned = conversionDto.order_cost * CommissionRate.
         * 4. Create CreateEarnedCommissionDTO entity with CreatorId, StoreId, ConversionId, OrderCost, AmtEarned.
         * 5. Call repository to save EarnedCommission entity.
         */
        throw new NotImplementedException();
    }
}