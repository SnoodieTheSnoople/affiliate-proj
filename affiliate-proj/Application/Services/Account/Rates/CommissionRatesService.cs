using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Services.Account.Rates;

public class CommissionRatesService : ICommissionRatesService
{
    public Task<CommissionRateDTO> SetCommissionRateAsync(CreateCommissionRateDTO createCommissionRateDTO)
    {
        throw new NotImplementedException();
    }
}