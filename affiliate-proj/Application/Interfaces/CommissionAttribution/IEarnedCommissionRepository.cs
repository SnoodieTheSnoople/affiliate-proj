using affiliate_proj.Core.DTOs.EarnedCommission;

namespace affiliate_proj.Application.Interfaces.CommissionAttribution;

public interface IEarnedCommissionRepository
{
    Task<EarnedCommissionDTO> SetEarnedCommission(CreateEarnedCommissionDTO createEarnedCommissionDto);
}