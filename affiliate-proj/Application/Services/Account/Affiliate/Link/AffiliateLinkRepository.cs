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
        
        var checkExists = await _dbContext.AffiliateLinks.FirstOrDefaultAsync(x => x.CreatorId == entity.CreatorId
                                                                     && x.StoreId == entity.StoreId &&
                                                                     x.RefParam == entity.RefParam &&
                                                                     x.ProductLink == entity.ProductLink);
        if (checkExists != null)
        {
            return ConvertEntityToDto(checkExists);
        }
        
        await _dbContext.AffiliateLinks.AddAsync(entity);
        await  _dbContext.SaveChangesAsync();
        
        return ConvertEntityToDto(await _dbContext.AffiliateLinks.FirstOrDefaultAsync(
            x => x.CreatorId == entity.CreatorId && x.StoreId == entity.StoreId &&
                 x.RefParam == entity.RefParam &&
                 x.ProductLink == entity.ProductLink));
    }
    
    public async Task<List<AffiliateLinkDTO>> GetAffiliateLinksByCreatorIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        var entities = await _dbContext.AffiliateLinks
            .Where(link => link.CreatorId == id)
            .Select(link => ConvertEntityToDto(link))
            .ToListAsync();
        
        return entities;
    }

    public async Task<List<AffiliateLinkDTO>> GetAffiliateLinksByStoreIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        var entities = await _dbContext.AffiliateLinks
            .Where(link => link.StoreId == id)
            .Select(link => ConvertEntityToDto(link))
            .ToListAsync();
        
        return entities;
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

    private AffiliateLinkDTO ConvertEntityToDto(AffiliateLink entity)
    {
        return new AffiliateLinkDTO
        {
            LinkId = entity.LinkId,
            CreatorId = entity.CreatorId,
            StoreId = entity.StoreId,
            Link = entity.Link,
            RefParam = entity.RefParam,
            ProductLink = entity.ProductLink,
            Clicks = entity.Clicks,
            CreatedAt = entity.CreatedAt
        };
    }
}