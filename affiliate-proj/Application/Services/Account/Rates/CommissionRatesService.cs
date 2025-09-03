using System.Data;
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

    public async Task<List<CommissionRateDTO>> GetCommissionRatesAsync(Guid id, char purposeType)
    {
        if (id == Guid.Empty)
            throw new NullReferenceException();

        if (purposeType == 'c') 
            return await _commissionRatesRepository.GetCommissionRatesByCreatorIdAsync(id);
        
        if (purposeType == 's')
            return await _commissionRatesRepository.GetCommissionRatesByStoreIdAsync(id);

        return new List<CommissionRateDTO>();;
    }

    public async Task<CommissionRateDTO?> GetCommissionRateByRateIdAsync(Guid rateId)
    {
        if (rateId == Guid.Empty)
            throw new NullReferenceException();

        return await _commissionRatesRepository.GetCommissionRateByRateIdAsync(rateId);
    }

    public async Task<CommissionRateDTO> UpdateCommissionRateAsync(CommissionRateDTO commissionRateDTO)
    {
        if (commissionRateDTO == null)
            throw new ArgumentNullException(nameof(commissionRateDTO));

        if (commissionRateDTO.RateId == Guid.Empty)
            throw new ArgumentNullException();
        
        if (commissionRateDTO.Rate > 100)
            throw new ArgumentOutOfRangeException(nameof(commissionRateDTO.Rate));
        
        var checkIfCurrentRateIsAccepted = await _commissionRatesRepository
            .GetCommissionRateByRateIdAsync(commissionRateDTO.RateId);
        
        if (checkIfCurrentRateIsAccepted.IsAccepted)
        {
            return await _commissionRatesRepository.SetCommissionRateAsync(new CommissionRate
            {
                CreatorId = checkIfCurrentRateIsAccepted.CreatorId,
                StoreId = checkIfCurrentRateIsAccepted.StoreId,
                Rate = commissionRateDTO.Rate,
                IsAccepted = false
            });
        }
        
        return await _commissionRatesRepository.UpdateCommissionRateAsync(commissionRateDTO.RateId, commissionRateDTO.Rate);
    }

    public async Task<CommissionRateDTO> AcceptCommissionRateAsync(CommissionRateDTO commissionRateDTO)
    {
        if (commissionRateDTO == null)
            throw new ArgumentNullException("Object null");

        if (commissionRateDTO.RateId == Guid.Empty)
            throw new ArgumentNullException();

        if (!commissionRateDTO.IsAccepted)
            throw new ArgumentException("Cannot be false");
        
        var checkIfCurrentRateIsAccepted = await _commissionRatesRepository
            .GetCommissionRateByRateIdAsync(commissionRateDTO.RateId);

        if (checkIfCurrentRateIsAccepted.IsAccepted)
        {
            var oldRate = await DeleteCommissionRateAsync(commissionRateDTO.RateId);
            if (oldRate != null)
                throw new DBConcurrencyException("Unable to delete record");
        }

        return await _commissionRatesRepository
            .AcceptCommissionRateAsync(commissionRateDTO.RateId, commissionRateDTO.IsAccepted);
    }

    public async Task<CommissionRateDTO?> DeleteCommissionRateAsync(Guid rateId)
    {
        if (rateId == Guid.Empty)
            throw new ArgumentNullException("ID empty");

        var checkRepoShowsNull = await _commissionRatesRepository.DeleteCommissionRateAsync(rateId);

        if (checkRepoShowsNull != null)
            throw new ArgumentException("Internal error. Unable to delete record.");

        return checkRepoShowsNull;
    }
}