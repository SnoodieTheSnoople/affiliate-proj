using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces.Account.Rates;

public interface ICommissionRatesRepository
{
    Task<CommissionRateDTO> SetCommissionRateAsync(CommissionRate commissionRate);
    Task<List<CommissionRateDTO>> GetCommissionRatesByCreatorIdAsync(Guid creatorId);
    Task<CommissionRateDTO> GetCommissionRatesByStoreIdAsync(Guid storeId);
    Task<CommissionRateDTO> GetCommissionRateByRateIdAsync(Guid rateId);
}