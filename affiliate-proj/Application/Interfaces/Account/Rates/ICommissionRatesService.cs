using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces.Account.Rates;

public interface ICommissionRatesService
{
    Task<CommissionRateDTO> SetCommissionRateAsync(CreateCommissionRateDTO createCommissionRateDTO);
    Task<CommissionRateDTO> GetCommissionRatesAsync(Guid id, char purposeType);
    Task<CommissionRateDTO> GetCommissionRateByRateIdAsync(Guid rateId);
}