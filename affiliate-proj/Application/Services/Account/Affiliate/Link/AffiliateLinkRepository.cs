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
            .AsNoTracking()
            .Where(link => link.CreatorId == id)
            .Select(link => new AffiliateLinkDTO
            {
                LinkId = link.LinkId,
                CreatorId = link.CreatorId,
                StoreId = link.StoreId,
                Link = link.Link,
                RefParam = link.RefParam,
                ProductLink = link.ProductLink,
                Clicks = link.Clicks,
                CreatedAt = link.CreatedAt
            })
            .ToListAsync();
        
        return entities;
    }

    public async Task<List<AffiliateLinkDTO>> GetAffiliateLinksByStoreIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        var entities = await _dbContext.AffiliateLinks
            .AsNoTracking()
            .Where(link => link.StoreId == id)
            .Select(link => new AffiliateLinkDTO
            {
                LinkId = link.LinkId,
                CreatorId = link.CreatorId,
                StoreId = link.StoreId,
                Link = link.Link,
                RefParam = link.RefParam,
                ProductLink = link.ProductLink,
                Clicks = link.Clicks,
                CreatedAt = link.CreatedAt
            })
            .ToListAsync();
        
        return entities;
    }

    public async Task<AffiliateLinkDTO?> GetAffiliateLinkByIdAsync(Guid linkId)
    {
        if (linkId == Guid.Empty)
            throw new ArgumentException("Link ID cannot be empty.", nameof(linkId));

        return await _dbContext.AffiliateLinks.FindAsync(linkId);
    }

    public async Task<AffiliateLinkDTO> UpdateAffiliateLinkAsync(AffiliateLinkDTO affiliateLinkDto)
    {
        var existingEntity = await _dbContext.AffiliateLinks.FindAsync(affiliateLinkDto.LinkId);
        
        if (existingEntity == null)
            throw new KeyNotFoundException("Affiliate link not found.");
        
        existingEntity.Link = affiliateLinkDto.Link;
        existingEntity.RefParam = affiliateLinkDto.RefParam;
        existingEntity.ProductLink = affiliateLinkDto.ProductLink;
        existingEntity.IsActive = affiliateLinkDto.IsActive;
        await _dbContext.SaveChangesAsync();
        
        existingEntity = await _dbContext.AffiliateLinks.FindAsync(affiliateLinkDto.LinkId);
        return ConvertEntityToDto(existingEntity);
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
    
    private AffiliateLink ConvertDtoToEntity(AffiliateLinkDTO affiliateLink)
    {
        return new AffiliateLink
        {
            LinkId = affiliateLink.LinkId,
            CreatorId = affiliateLink.CreatorId,
            StoreId = affiliateLink.StoreId,
            Link = affiliateLink.Link,
            RefParam = affiliateLink.RefParam,
            ProductLink = affiliateLink.ProductLink,
            Clicks = affiliateLink.Clicks,
            IsActive = affiliateLink.IsActive
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