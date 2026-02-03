using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Core.DTOs.EarnedCommission;
using affiliate_proj.Core.Entities;

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
        throw new NotImplementedException();
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
}