using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Core.DTOs.Affiliate.Link;

namespace affiliate_proj.Application.Services.Account.Affiliate.Link;

public class AffiliateLinkService : IAffiliateLinkService
{
    public async Task<AffiliateLinkDTO?> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto)
    {
        // Validate if Creator and Store exists
        // Create new link and use baseUrl + refParam
        // Get ProductLink and validate exists in db
        // Set Clicks to 0
        
        
        throw new NotImplementedException();
    }
}