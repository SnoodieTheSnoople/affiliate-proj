using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Link;

public interface IAffiliateLinkRepository
{
    Task<AffiliateLinkDTO> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto);
}