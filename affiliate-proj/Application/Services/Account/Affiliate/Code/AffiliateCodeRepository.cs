using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Core.DTOs.Affiliate.Code;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Account.Affiliate.Code;

public class AffiliateCodeRepository : IAffiliateCodeRepository
{
    private readonly PostgresDbContext _dbContext;

    public AffiliateCodeRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AffiliateCodeDTO> SetAffiliateCodeAsync(CreateAffiliateCodeDTO createAffiliateCodeDto)
    {
        var entity = ConvertDtoToEntity(createAffiliateCodeDto);
        
        var checkExists = await _dbContext.AffiliateCodes.FirstOrDefaultAsync(x =>
            x.CreatorId == entity.CreatorId && x.StoreId == entity.StoreId && x.Code == entity.Code
            && x.ProductLink == entity.ProductLink);

        if (checkExists != null)
        {
            return ConvertEntityToDto(checkExists);
        }
        
        await _dbContext.AffiliateCodes.AddAsync(entity);
        await  _dbContext.SaveChangesAsync();
        
        return ConvertEntityToDto(await _dbContext.AffiliateCodes.FirstOrDefaultAsync(x =>
            x.CreatorId == entity.CreatorId && x.StoreId == entity.StoreId && x.Code == entity.Code
            && x.ProductLink == entity.ProductLink));
    }

    public async Task<AffiliateCodeDTO?> GetAffiliateCodeAsync(string code)
    {
        if (String.IsNullOrEmpty(code)) 
            throw new ArgumentException("Code cannot be null or empty", nameof(code));

        return ConvertEntityToDto(await _dbContext.AffiliateCodes.FirstOrDefaultAsync(x => x.Code == code));
    }
    
    private AffiliateCode ConvertDtoToEntity(CreateAffiliateCodeDTO dto)
    {
        return new AffiliateCode
        {
            CreatorId = dto.CreatorId,
            StoreId = dto.StoreId,
            Code = dto.Code,
            IsActive = dto.IsActive,
            ValidFor = dto.ValidFor,
            ExpiryDate = dto.ExpiryDate,
            ProductLink = dto.ProductLink
        };
    }
    
    private AffiliateCodeDTO? ConvertEntityToDto(AffiliateCode? entity)
    {
        if (entity == null)
            return null;

        return new AffiliateCodeDTO
        {
            CodeId = entity.CodeId,
            CreatorId = entity.CreatorId,
            StoreId = entity.StoreId,
            Code = entity.Code,
            IsActive = entity.IsActive,
            ValidFor = entity.ValidFor,
            ExpiryDate = entity.ExpiryDate,
            CreatedAt = entity.CreatedAt,
            ProductLink = entity.ProductLink
        };
    }
}