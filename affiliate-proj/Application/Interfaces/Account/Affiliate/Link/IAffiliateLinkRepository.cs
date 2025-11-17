using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Link;

public interface IAffiliateLinkRepository
{
    Task<AffiliateLinkDTO> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto);
    Task<List<AffiliateLinkDTO>> GetAffiliateLinksByCreatorIdAsync(Guid id);
    Task<List<AffiliateLinkDTO>> GetAffiliateLinksByStoreIdAsync(Guid id);
    Task<List<AffiliateLinkDTO>> GetAffiliateLinkByIdAsync(Guid linkId);
}