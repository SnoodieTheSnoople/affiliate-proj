using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Services.Account.Affiliate.Link;

public class AffiliateLinkService : IAffiliateLinkService
{
    private readonly IAccountHelper _accountHelper;
    private readonly IStoreService _storeService;
    private readonly IConfiguration _configuration;

    public AffiliateLinkService(IAccountHelper accountHelper, IStoreService storeService, IConfiguration configuration)
    {
        _accountHelper = accountHelper;
        _storeService = storeService;
        _configuration = configuration;
    }

    public async Task<AffiliateLinkDTO?> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto)
    {
        // Validate if Creator and Store exists
        // Create new link and use baseUrl + refParam
        // Get ProductLink and validate exists in db
        // Set Clicks to 0
        
        if (!_accountHelper.CheckUserExists(createAffiliateLinkDto.CreatorId))
        {
            throw new Exception("Creator does not exist.");
        }
        await _storeService.GetStoreByIdAsync(createAffiliateLinkDto.StoreId); // Will throw if not found anyway
        
        // Link validation
        var baseUrl = new Uri(_configuration.GetValue<string>("BaseUrl"));
        var affiliateLinkUri = new Uri(createAffiliateLinkDto.Link);
        var isSchemeSame = affiliateLinkUri.Scheme == baseUrl.Scheme;
        var isHostSame = affiliateLinkUri.Host == baseUrl.Host;
        
        var path = affiliateLinkUri.AbsolutePath.Trim('/');
        var isValidPath = !string.IsNullOrEmpty(path) && !path.Contains(createAffiliateLinkDto.RefParam);
        
        if (!isSchemeSame || !isHostSame || !isValidPath)
        {
            throw new Exception("Invalid link.");
        }
        
        throw new NotImplementedException();
    }
}