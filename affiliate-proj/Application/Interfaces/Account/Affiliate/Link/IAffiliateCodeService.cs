using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Link;

public interface IAffiliateCodeService
{
    Task<AffiliateCodeDTO> SetAffiliateCodeAsync(CreateAffiliateCodeDTO createAffiliateCodeDto);
}