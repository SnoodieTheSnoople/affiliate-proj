using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Services.Account.Affiliate.Code;

public class AffiliateCodeService : IAffiliateCodeService
{
    private readonly ICreatorService _creatorService;
    private readonly IStoreService _storeService;
    private readonly ILogger<AffiliateCodeService> _logger;

    public AffiliateCodeService(ICreatorService creatorService, ILogger<AffiliateCodeService> logger, 
        IStoreService storeService)
    {
        _creatorService = creatorService;
        _logger = logger;
        _storeService = storeService;
    }

    public async Task<AffiliateCodeDTO> SetAffiliateCodeAsync(CreateAffiliateCodeDTO createAffiliateCodeDto)
    {
        // Validate if Creator and Store exists
        // Get ProductLink and validate exists in db
        // Validate code uniqueness
        // Validate date expiry using the ValidFor field
        // Create new code entry in db
        
        if (!await _creatorService.CheckCreatorExistsAsync(createAffiliateCodeDto.CreatorId))
        {
            _logger.LogError("Creator not found: {creatorId}", createAffiliateCodeDto.CreatorId);
            throw new Exception("Creator does not exist.");
        }
        
        throw new NotImplementedException();
    }
}