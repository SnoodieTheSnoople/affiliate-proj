using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Interfaces.Account.Affiliate.Code;

public interface IAffiliateCodeRepository
{
    Task<AffiliateCodeDTO?> GetAffiliateCodeAsync(string code);
}