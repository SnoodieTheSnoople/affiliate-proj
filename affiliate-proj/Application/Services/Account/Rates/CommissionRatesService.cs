using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Services.Account.Rates;

public class CommissionRatesService : ICommissionRatesService
{
    private readonly ICommissionRatesRepository _commissionRatesRepository;

    public CommissionRatesService(ICommissionRatesRepository commissionRatesRepository)
    {
        _commissionRatesRepository = commissionRatesRepository;
    }

    public async Task<CommissionRateDTO> SetCommissionRateAsync(CreateCommissionRateDTO createCommissionRateDTO)
    {
        if (createCommissionRateDTO == null)
            throw new ArgumentNullException(nameof(createCommissionRateDTO));
        
        if (createCommissionRateDTO.Rate > 100)
            throw new ArgumentOutOfRangeException(nameof(createCommissionRateDTO.Rate));
        
        var commissionRate = new CommissionRate
        {
            CreatorId = createCommissionRateDTO.CreatorId,
            StoreId = createCommissionRateDTO.StoreId,
            Rate = createCommissionRateDTO.Rate,
            IsAccepted = createCommissionRateDTO.IsAccepted
        };

        return await _commissionRatesRepository.SetCommissionRateAsync(commissionRate);
    }
}