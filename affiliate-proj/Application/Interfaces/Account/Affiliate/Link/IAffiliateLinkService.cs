using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Link;

public interface IAffiliateLinkService
{
    Task<AffiliateLinkDTO?> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto);
    Task<List<AffiliateLinkDTO>?> GetAffiliateLinksByCreatorIdAsync(Guid creatorId);
    Task<List<AffiliateLinkDTO>?> GetAffiliateLinksByStoreIdAsync(Guid storeId);
    Task<AffiliateLinkDTO?> GetAffiliateLinkByIdAsync(Guid affiliateLinkId);
}