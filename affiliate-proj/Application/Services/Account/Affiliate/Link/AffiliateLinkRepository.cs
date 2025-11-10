using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Link;
using affiliate_proj.Core.DTOs.Affiliate.Link;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Account.Affiliate.Link;

public class AffiliateLinkRepository : IAffiliateLinkRepository
{
    private readonly PostgresDbContext _dbContext;

    public AffiliateLinkRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AffiliateLinkDTO> SetAffiliateLinkAsync(CreateAffiliateLinkDTO createAffiliateLinkDto)
    {
        var entity = ConvertDtoToEntity(createAffiliateLinkDto);

        if (await _dbContext.AffiliateLinks.FirstOrDefaultAsync(x => x.CreatorId == entity.CreatorId
                                                                     && x.StoreId == entity.StoreId &&
                                                                     x.RefParam == entity.RefParam &&
                                                                     x.ProductLink == entity.ProductLink) != null)
        {
            throw new Exception("Affiliate link already exists.");
        }
        
        throw new NotImplementedException();
    }
    
    private AffiliateLink ConvertDtoToEntity(CreateAffiliateLinkDTO affiliateLink)
    {
        return new AffiliateLink
        {
            CreatorId = affiliateLink.CreatorId,
            StoreId = affiliateLink.StoreId,
            Link = affiliateLink.Link,
            RefParam = affiliateLink.RefParam,
            ProductLink = affiliateLink.ProductLink,
            Clicks = affiliateLink.Clicks
        };
    }
}