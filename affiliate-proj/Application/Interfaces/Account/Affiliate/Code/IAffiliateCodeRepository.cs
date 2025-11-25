using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Code;

public interface IAffiliateCodeRepository
{
    Task<AffiliateCodeDTO> SetAffiliateCodeAsync(CreateAffiliateCodeDTO createAffiliateCodeDto);
    Task<AffiliateCodeDTO?> GetAffiliateCodeAsync(string code);
    Task<List<AffiliateCodeDTO>> GetAffiliateCodesByCreatorIdAsync(Guid creatorId);
    Task<List<AffiliateCodeDTO>> GetAffiliateCodesByStoreIdAsync(Guid storeId);
    Task<AffiliateCodeDTO?> GetAffiliateCodeByIdAsync(Guid codeId);
    Task<AffiliateCodeDTO> UpdateAffiliateCodeAsync(AffiliateCodeDTO affiliateCodeDto);
    Task<AffiliateCodeDTO> UpdateAffiliateCodeStatusAsync(Guid codeId, bool isActive);
    Task<AffiliateCodeDTO?> DeleteAffiliateCodeAsync(Guid codeId);
}