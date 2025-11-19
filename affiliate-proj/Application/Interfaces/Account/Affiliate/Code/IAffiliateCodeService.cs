using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Code;

public interface IAffiliateCodeService
{
    Task<AffiliateCodeDTO> SetAffiliateCodeAsync(CreateAffiliateCodeDTO createAffiliateCodeDto);
    Task<List<AffiliateCodeDTO>> GetAffiliateCodesByCreatorIdAsync();
    Task<List<AffiliateCodeDTO>> GetAffiliateCodesByStoreIdAsync();
}