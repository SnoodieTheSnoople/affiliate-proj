using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Services.Account.Affiliate.Link;

public class AffiliateLinkService : IAffiliateLinkService
{
    private readonly IAffiliateLinkRepository _affiliateLinkRepository;
    private readonly IAccountHelper _accountHelper;
    private readonly IStoreService _storeService;
    private readonly IShopifyProductRepository _shopifyProductRepository;
    private readonly IConfiguration _configuration;

    public AffiliateLinkService(IAccountHelper accountHelper, IStoreService storeService, IConfiguration configuration, IShopifyProductRepository shopifyProductRepository, IAffiliateLinkRepository affiliateLinkRepository)
    {
        _accountHelper = accountHelper;
        _storeService = storeService;
        _configuration = configuration;
        _shopifyProductRepository = shopifyProductRepository;
        _affiliateLinkRepository = affiliateLinkRepository;
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
        // TODO: Move to separate validator method. Refactor for security and depth.
        var baseUrl = new Uri(_configuration.GetValue<string>("BaseUrl"));
        var affiliateLinkUri = new Uri(createAffiliateLinkDto.Link);
        var isSchemeSame = affiliateLinkUri.Scheme == baseUrl.Scheme;
        var isHostSame = affiliateLinkUri.Host == baseUrl.Host;
        
        var path = affiliateLinkUri.AbsolutePath.Trim('/');
        var isValidPath = !string.IsNullOrEmpty(path) && path.Contains(createAffiliateLinkDto.RefParam);
        
        if (!isSchemeSame || !isHostSame || !isValidPath)
        {
            throw new Exception("Invalid link.");
        }
        
        // TODO: Implement Shopify ProductLink validation
        if (await _shopifyProductRepository.CheckShopifyProductExistsByLinkAsync(createAffiliateLinkDto.ProductLink, 
                createAffiliateLinkDto.StoreId) == null)
        {
            throw new Exception("Product link does not exist.");
        }

        createAffiliateLinkDto.Clicks = 0;

        await _affiliateLinkRepository.SetAffiliateLinkAsync(createAffiliateLinkDto);
        
        throw new NotImplementedException();
    }
}