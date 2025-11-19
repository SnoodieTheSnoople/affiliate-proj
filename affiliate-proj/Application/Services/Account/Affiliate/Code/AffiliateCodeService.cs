using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Services.Account.Affiliate.Code;

public class AffiliateCodeService : IAffiliateCodeService
{
    private readonly IAffiliateCodeRepository _affiliateCodeRepository;
    private readonly ICreatorService _creatorService;
    private readonly IStoreService _storeService;
    private readonly ILogger<AffiliateCodeService> _logger;

    public AffiliateCodeService(ICreatorService creatorService, ILogger<AffiliateCodeService> logger, 
        IStoreService storeService, IAffiliateCodeRepository affiliateCodeRepository)
    {
        _creatorService = creatorService;
        _logger = logger;
        _storeService = storeService;
        _affiliateCodeRepository = affiliateCodeRepository;
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
        
        _logger.LogInformation("Creator found: {CreatorId}", createAffiliateCodeDto.CreatorId);
        
        await _storeService.GetStoreByIdAsync(createAffiliateCodeDto.StoreId);
        _logger.LogInformation("Store found: {StoreId}", createAffiliateCodeDto.StoreId);

        if (await _affiliateCodeRepository.GetAffiliateCodeAsync(createAffiliateCodeDto.Code) != null)
        {
            _logger.LogInformation("Affiliate code found: {Code}", createAffiliateCodeDto.Code);
            throw new Exception("Affiliate code already exists.");
        }
        
        var expiryDate = DateTime.UtcNow.Date.AddDays(createAffiliateCodeDto.ValidFor);
        if (expiryDate != createAffiliateCodeDto.ExpiryDate.Date)
        {
            _logger.LogInformation("Invalid expiry date for code: {expiryDate}", expiryDate);
            throw new Exception("Invalid expiry date.");
        }
        
        return await _affiliateCodeRepository.SetAffiliateCodeAsync(createAffiliateCodeDto);
    }

    public async Task<List<AffiliateCodeDTO>> GetAffiliateCodesByCreatorIdAsync(Guid creatorId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AffiliateCodeDTO>> GetAffiliateCodesByStoreIdAsync(Guid storeId)
    {
        throw new NotImplementedException();
    }
}