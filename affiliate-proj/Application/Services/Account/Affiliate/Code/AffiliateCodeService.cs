using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Services.Account.Affiliate.Code;

public class AffiliateCodeService : IAffiliateCodeService
{
    private readonly ICreatorService _creatorService;

    public AffiliateCodeService(ICreatorService creatorService)
    {
        _creatorService = creatorService;
    }

    public async Task<AffiliateCodeDTO> SetAffiliateCodeAsync(CreateAffiliateCodeDTO createAffiliateCodeDto)
    {
        // Validate if Creator and Store exists
        // Get ProductLink and validate exists in db
        // Validate code uniqueness
        // Validate date expiry using the ValidFor field
        // Create new code entry in db
        
        throw new NotImplementedException();
    }
}