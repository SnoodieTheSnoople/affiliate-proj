using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Core.DTOs.EarnedCommission;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.CommissionAttribution;

public class EarnedCommissionRepository : IEarnedCommissionRepository
{
    private readonly PostgresDbContext _dbContext;

    public EarnedCommissionRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EarnedCommissionDTO> SetEarnedCommission(CreateEarnedCommissionDTO createEarnedCommissionDto)
    {
        var entity = ConvertDtoToEntity(createEarnedCommissionDto);
        
        var checkExists = await _dbContext.EarnedCommissions.FirstOrDefaultAsync(x =>
            x.ConversionId == entity.ConversionId);

        if (checkExists != null)
        {
            return ConvertEntityToDto(checkExists);
        }
        
        await _dbContext.EarnedCommissions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        checkExists = await _dbContext.EarnedCommissions.FirstOrDefaultAsync(x =>
            x.ConversionId == entity.ConversionId);
        
        return ConvertEntityToDto(checkExists);
    }
    
    private EarnedCommission ConvertDtoToEntity(CreateEarnedCommissionDTO dto)
    {
        return new EarnedCommission
        {
            CreatorId = dto.CreatorId,
            StoreId = dto.StoreId,
            ConversionId = dto.ConversionId,
            OrderCost = dto.OrderCost,
            AmtEarned = dto.AmtEarned
        };
    }

    private EarnedCommissionDTO ConvertEntityToDto(EarnedCommission entity)
    {
        return new EarnedCommissionDTO
        {
            CommissionId = entity.ConversionId,
            CreatorId = entity.CreatorId,
            StoreId = entity.StoreId,
            ConversionId = entity.ConversionId,
            OrderCost = entity.OrderCost,
            AmtEarned = entity.AmtEarned
        };
    }
}