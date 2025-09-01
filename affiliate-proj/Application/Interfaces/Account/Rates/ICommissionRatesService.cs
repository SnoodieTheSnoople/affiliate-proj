using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces.Account.Rates;

public interface ICommissionRatesService
{
    Task<CommissionRate>  SetCommissionRateAsync(CreateCommissionRateDTO createCommissionRateDTO);
}