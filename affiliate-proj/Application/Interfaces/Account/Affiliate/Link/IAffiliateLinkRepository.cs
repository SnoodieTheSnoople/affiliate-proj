using affiliate_proj.Core.DTOs.Affiliate.Link;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Link;

public interface IAffiliateLinkRepository
{
    Task<AffiliateLinkDTO> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto);
    Task<List<AffiliateLinkDTO>> GetAffiliateLinksByCreatorIdAsync(Guid id);
    Task<List<AffiliateLinkDTO>> GetAffiliateLinksByStoreIdAsync(Guid id);
    Task<AffiliateLink?> GetAffiliateLinkByIdAsync(Guid linkId);
    Task<AffiliateLinkDTO> UpdateAffiliateLinkAsync(AffiliateLinkDTO affiliateLinkDto);
}