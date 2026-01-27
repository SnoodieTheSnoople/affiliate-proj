using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
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

    private readonly IConversionService _conversionService;
    private readonly ICreatorService _creatorService;
    private readonly IAffiliateCodeService _affiliateCodeService;
    private readonly IAffiliateLinkService _affiliateLinkService;

    public EarnedCommissionService(IConversionService conversionService, ICreatorService creatorService, IAffiliateCodeService affiliateCodeService, IAffiliateLinkService affiliateLinkService)
    {
        _conversionService = conversionService;
        _creatorService = creatorService;
        _affiliateCodeService = affiliateCodeService;
        _affiliateLinkService = affiliateLinkService;
    }

    public async Task CalculateAttributedCommissionAsync(ConversionDTO conversionDto)
    {
        /*
         * 1. Lookup affiliate_code or landing_site/landing_site_ref to identify the creator. Fetch CreatorId.
         * 2. Lookup CommissionRates based on StoreId and CreatorId to get the commission rate.
         * 3. Calculate commission: AmtEarned = conversionDto.order_cost * CommissionRate.
         * 4. Create CreateEarnedCommissionDTO entity with CreatorId, StoreId, ConversionId, OrderCost, AmtEarned.
         * 5. Call repository to save EarnedCommission entity.
         */
        
        var creatorId = Guid.Empty;

        if (!String.IsNullOrEmpty(conversionDto.Code))
        {
            // Lookup affiliate code to get CreatorId
            creatorId = (await _affiliateCodeService.GetAffiliateCodeByCodeAsync(conversionDto.Code)).CreatorId;
        }
        else if (!String.IsNullOrEmpty(conversionDto.LandingSite) && !String.IsNullOrEmpty(conversionDto.LandingSiteRef))
        {
            // Lookup landing site/ref to get CreatorId
            creatorId = (await _affiliateLinkService.GetAffiliateLinkByLinkAsync(conversionDto.LandingSite)).CreatorId;
        }
        else
        {
            // No attribution possible
            return;
        }
        
        throw new NotImplementedException();
    }
}