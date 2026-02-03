using affiliate_proj.Application.Interfaces.CommissionAttribution;
using affiliate_proj.Core.DTOs.EarnedCommission;

namespace affiliate_proj.Application.Services.CommissionAttribution;

public class EarnedCommissionRepository : IEarnedCommissionRepository
{
    public async Task<EarnedCommissionDTO> SetEarnedCommission(CreateEarnedCommissionDTO createEarnedCommissionDto)
    {
        throw new NotImplementedException();
    }
}