using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Application.Interfaces.Shopify.Webhook.Conversion;
using affiliate_proj.Core.DTOs.EarnedCommission;
using affiliate_proj.Core.DTOs.Rates;
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
    private readonly ICommissionRatesService _commissionRatesService;
    private readonly ILogger<EarnedCommissionService> _logger;
    private readonly IEarnedCommissionRepository _earnedCommissionRepository;

    public EarnedCommissionService(IConversionService conversionService, ICreatorService creatorService, 
        IAffiliateCodeService affiliateCodeService, IAffiliateLinkService affiliateLinkService, 
        ICommissionRatesService commissionRatesService, ILogger<EarnedCommissionService> logger, IEarnedCommissionRepository earnedCommissionRepository)
    {
        _conversionService = conversionService;
        _creatorService = creatorService;
        _affiliateCodeService = affiliateCodeService;
        _affiliateLinkService = affiliateLinkService;
        _commissionRatesService = commissionRatesService;
        _logger = logger;
        _earnedCommissionRepository = earnedCommissionRepository;
    }

    public async Task<Decimal> CalculateAttributedCommissionAsync(ConversionDTO conversionDto, Guid creatorId)
    {
        /*// 1. Lookup affiliate_code or landing_site/landing_site_ref to identify the creator. Fetch CreatorId.
        var creatorId = Guid.Empty;

        if (!String.IsNullOrEmpty(conversionDto.Code))
        {
            // Lookup affiliate code to get CreatorId
            _logger.LogInformation("Code: {code}", conversionDto.Code);
            creatorId = (await _affiliateCodeService.GetAffiliateCodeByCodeAsync(conversionDto.Code)).CreatorId;
            _logger.LogInformation("CreatorId from code: {creatorId}", creatorId);
        }
        else if (!String.IsNullOrEmpty(conversionDto.LandingSite) && !String.IsNullOrEmpty(conversionDto.LandingSiteRef))
        {
            // Lookup landing site/ref to get CreatorId
            _logger.LogInformation("LandingSite: {landingSite}, LandingSiteRef: {landingSiteRef}", conversionDto.LandingSite, conversionDto.LandingSiteRef);
            creatorId = (await _affiliateLinkService.GetAffiliateLinkByLinkAsync(conversionDto.LandingSite)).CreatorId;
            _logger.LogInformation("CreatorId from landing site/ref: {creatorId}", creatorId);
        }
        else
        {
            // No attribution possible
            return;
        }
        */
        
        // 2. Lookup CommissionRates based on StoreId and CreatorId to get the commission rate.
        _logger.LogInformation("Getting rate for CreatorId: {creatorId} and StoreId: {storeId}", creatorId, conversionDto.StoreId);
        var rate = await _commissionRatesService.GetCommissionRateByCreatorAndStoreIdsAsync(creatorId, conversionDto.StoreId);
        _logger.LogInformation("RateId: {rateId}", rate.RateId);
        
        
        // 3. Calculate commission: AmtEarned = conversionDto.order_cost * CommissionRate.
        
        _logger.LogInformation("OrderCost: {orderCost}, Commission Rate: {commissionRate}", 
            conversionDto.OrderCost, rate.Rate);
        var commissionAmount = conversionDto.OrderCost * ((decimal) rate.Rate / 100);
        _logger.LogInformation("Attributed Commission Rate: {commissionAmount}", commissionAmount);

        return commissionAmount;

        /*return commissionAmount;

        // 4. Create CreateEarnedCommissionDTO entity with CreatorId, StoreId, ConversionId, OrderCost, AmtEarned.
        var newEarnedCommission = new CreateEarnedCommissionDTO
        {
            CreatorId = creatorId,
            StoreId = conversionDto.StoreId,
            ConversionId = conversionDto.ConversionId,
            OrderCost = conversionDto.OrderCost,
            AmtEarned = commissionAmount
        };

        // 5. Call repository to save EarnedCommission entity.
        await _earnedCommissionRepository.SetEarnedCommission(newEarnedCommission);*/

        // TODO: Separate concerns. Keep this method for calculation only.
    }

    public async Task SetEarnedCommissionAsync(ConversionDTO conversionDto)
    {
        var creatorId = await GetCreatorIdFromConversionAsync(conversionDto);
        
        if (creatorId == Guid.Empty)
        {
            _logger.LogInformation("No creator attribution possible for ConversionId: {conversionId}", conversionDto.ConversionId);
            return;
        }
        
        var commissionAmount = await CalculateAttributedCommissionAsync(conversionDto, creatorId);
        
        // 4. Create CreateEarnedCommissionDTO entity with CreatorId, StoreId, ConversionId, OrderCost, AmtEarned.
        var newEarnedCommission = new CreateEarnedCommissionDTO
        {
            CreatorId = creatorId,
            StoreId = conversionDto.StoreId,
            ConversionId = conversionDto.ConversionId,
            OrderCost = conversionDto.OrderCost,
            AmtEarned = commissionAmount
        };
        
        // 5. Call repository to save EarnedCommission entity.
        await _earnedCommissionRepository.SetEarnedCommission(newEarnedCommission);
        
        // TODO: Test and refactor
    }

    private async Task<Guid> GetCreatorIdFromConversionAsync(ConversionDTO conversionDto)
    {
        // 1. Lookup affiliate_code or landing_site/landing_site_ref to identify the creator. Fetch CreatorId.
        var creatorId = Guid.Empty;

        if (!String.IsNullOrEmpty(conversionDto.Code))
        {
            // Lookup affiliate code to get CreatorId
            _logger.LogInformation("Code: {code}", conversionDto.Code);
            return (await _affiliateCodeService.GetAffiliateCodeByCodeAsync(conversionDto.Code)).CreatorId;
        }

        if (!String.IsNullOrEmpty(conversionDto.LandingSite) && !String.IsNullOrEmpty(conversionDto.LandingSiteRef))
        {
            // Lookup landing site/ref to get CreatorId
            _logger.LogInformation("LandingSite: {landingSite}, LandingSiteRef: {landingSiteRef}", conversionDto.LandingSite, conversionDto.LandingSiteRef);
            return (await _affiliateLinkService.GetAffiliateLinkByLinkAsync(conversionDto.LandingSite)).CreatorId;
        }

        // No attribution possible
        return creatorId;
    }
}