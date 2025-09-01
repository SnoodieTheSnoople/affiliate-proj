using affiliate_proj.Core.DTOs.Rates;

namespace affiliate_proj.Application.Interfaces.Account.Rates;

public interface ICommissionRatesRepository
{
    Task<CreateCommissionRateDTO> SetCommissionRateAsync(CommissionRateDTO commissionRate);
}