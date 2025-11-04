using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Services.Account.Affiliate.Link;

public class AffiliateLinkService : IAffiliateLinkService
{
    private readonly IAccountHelper _accountHelper;
    private readonly IStoreService _storeService;

    public AffiliateLinkService(IAccountHelper accountHelper, IStoreService storeService)
    {
        _accountHelper = accountHelper;
        _storeService = storeService;
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
        
        
        
        throw new NotImplementedException();
    }
}