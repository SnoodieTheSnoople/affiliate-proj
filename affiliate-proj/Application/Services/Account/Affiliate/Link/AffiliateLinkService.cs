using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Services.Account.Affiliate.Link;

public class AffiliateLinkService : IAffiliateLinkService
{
    private readonly IAffiliateLinkRepository _affiliateLinkRepository;
    private readonly IStoreService _storeService;
    private readonly ICreatorService _creatorService;
    private readonly IShopifyProductRepository _shopifyProductRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AffiliateLinkService> _logger;

    public AffiliateLinkService(IAccountHelper accountHelper, IStoreService storeService, IConfiguration configuration, IShopifyProductRepository shopifyProductRepository, IAffiliateLinkRepository affiliateLinkRepository, ILogger<AffiliateLinkService> logger, ICreatorService creatorService)
    {
        _storeService = storeService;
        _configuration = configuration;
        _shopifyProductRepository = shopifyProductRepository;
        _affiliateLinkRepository = affiliateLinkRepository;
        _logger = logger;
        _creatorService = creatorService;
    }

    public async Task<AffiliateLinkDTO?> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto)
    {
        // Validate if Creator and Store exists
        // Create new link and use baseUrl + refParam
        // Get ProductLink and validate exists in db
        // Set Clicks to 0
        
        if (!await _creatorService.CheckCreatorExistsAsync(createAffiliateLinkDto.CreatorId))
        {
            _logger.LogError("Creator not found: {creatorId}", createAffiliateLinkDto.CreatorId);
            throw new Exception("Creator does not exist.");
        }
        
        _logger.LogInformation("Creator found: {CreatorId}", createAffiliateLinkDto.CreatorId);
        
        await _storeService.GetStoreByIdAsync(createAffiliateLinkDto.StoreId); // Will throw if not found anyway
        _logger.LogInformation("Store found: {StoreId}", createAffiliateLinkDto.StoreId);
        
        // Link validation
        // TODO: Move to separate validator method. Refactor for security and depth.
        var baseUrl = new Uri(_configuration.GetValue<string>("Shopify:BaseUrl"));
        var affiliateLinkUri = new Uri(createAffiliateLinkDto.Link);
        var isSchemeSame = affiliateLinkUri.Scheme == baseUrl.Scheme;
        var isHostSame = affiliateLinkUri.Host == baseUrl.Host;
        
        var path = affiliateLinkUri.AbsolutePath.Trim('/');
        var isValidPath = !string.IsNullOrEmpty(path) && path.Contains(createAffiliateLinkDto.RefParam);
        
        if (!isSchemeSame || !isHostSame || !isValidPath)
        {
            _logger.LogError("Invalid affiliate link: {link}", createAffiliateLinkDto.Link);
            throw new Exception("Invalid link.");
        }
        
        if (await _shopifyProductRepository.CheckShopifyProductExistsByLinkAsync(createAffiliateLinkDto.ProductLink, 
                createAffiliateLinkDto.StoreId) == null)
        {
            _logger.LogError("Shopify product not found.");
            throw new Exception("Product link does not exist.");
        }

        createAffiliateLinkDto.Clicks = 0;


        _logger.LogInformation("Affiliate link created for CreatorId: {creatorId}, StoreId: {storeId}",
            createAffiliateLinkDto.CreatorId, createAffiliateLinkDto.StoreId);
        _logger.LogInformation("Link set to {link}", createAffiliateLinkDto.Link);
        _logger.LogInformation("RefParam set to {refParam}", createAffiliateLinkDto.RefParam);
        _logger.LogInformation("ProductLink set to {productLink}", createAffiliateLinkDto.ProductLink);
        _logger.LogInformation("Clicks initialized to {clicks}", createAffiliateLinkDto.Clicks);
        
        return await _affiliateLinkRepository.SetAffiliateLinkAsync(createAffiliateLinkDto);
    }

    public async Task<List<AffiliateLinkDTO>?> GetAffiliateLinksByCreatorIdAsync(Guid creatorId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AffiliateLinkDTO>?> GetAffiliateLinksByStoreIdAsync(Guid storeId)
    {
        throw new NotImplementedException();
    }
}