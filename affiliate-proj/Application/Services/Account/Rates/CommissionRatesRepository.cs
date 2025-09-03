using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Account.Rates;
using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Account.Rates;

public class CommissionRatesRepository : ICommissionRatesRepository
{
    private readonly PostgresDbContext _postgresDbContext;

    public CommissionRatesRepository(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<CommissionRateDTO> SetCommissionRateAsync(CommissionRate commissionRate)
    {
        var checkRateExists = await _postgresDbContext.CommissionRates
            .Where(rate => rate.CreatorId == commissionRate.CreatorId)
            .Where(rate => rate.StoreId == commissionRate.StoreId)
            .FirstOrDefaultAsync();
        
        if (checkRateExists != null)
            throw new Exception("Commission rate already exists");
        
        await _postgresDbContext.CommissionRates.AddAsync(commissionRate);
        await _postgresDbContext.SaveChangesAsync();
        
        checkRateExists = await _postgresDbContext.CommissionRates
            .Where(rate => rate.CreatorId == commissionRate.CreatorId)
            .Where(rate => rate.StoreId == commissionRate.StoreId)
            .FirstOrDefaultAsync();

        return new CommissionRateDTO
        {
            RateId = checkRateExists.RateId,
            CreatedAt = checkRateExists.CreatedAt,
            CreatorId = checkRateExists.CreatorId,
            StoreId = checkRateExists.StoreId,
            Rate = checkRateExists.Rate,
            IsAccepted = checkRateExists.IsAccepted,
        };
    }

    public async Task<List<CommissionRateDTO>> GetCommissionRatesByCreatorIdAsync(Guid creatorId)
    {
        if (creatorId == Guid.Empty)
            throw new NullReferenceException();
        
        return await _postgresDbContext.CommissionRates.Select(rate => new CommissionRateDTO
            {
                RateId = rate.RateId,
                CreatedAt = rate.CreatedAt,
                CreatorId = rate.CreatorId,
                StoreId = rate.StoreId,
                Rate = rate.Rate,
                IsAccepted = rate.IsAccepted,
            })
            .Where(rate => rate.CreatorId == creatorId)
            .ToListAsync();
    }

    public async Task<List<CommissionRateDTO>> GetCommissionRatesByStoreIdAsync(Guid storeId)
    {
        if (storeId == Guid.Empty)
            throw new NullReferenceException();
        
        return await _postgresDbContext.CommissionRates.Select(rate => new CommissionRateDTO
            {
                RateId = rate.RateId,
                CreatedAt = rate.CreatedAt,
                CreatorId = rate.CreatorId,
                StoreId = rate.StoreId,
                Rate = rate.Rate,
                IsAccepted = rate.IsAccepted,
            })
            .Where(rate => rate.StoreId == storeId)
            .ToListAsync();
    }

    public async Task<CommissionRateDTO?> GetCommissionRateByRateIdAsync(Guid rateId)
    {
        if (rateId == Guid.Empty)
            throw new NullReferenceException();
        
        var result = await _postgresDbContext.CommissionRates.FindAsync(rateId);

        return new CommissionRateDTO
        {
            RateId = result.RateId,
            CreatedAt = result.CreatedAt,
            CreatorId = result.CreatorId,
            StoreId = result.StoreId,
            Rate = result.Rate,
            IsAccepted = result.IsAccepted,
        };
    }

    public async Task<CommissionRateDTO> UpdateCommissionRateAsync(Guid rateId, float rate)
    {
        if (rateId == Guid.Empty)
            throw new NullReferenceException();
        
        var commissionRateEntry = _postgresDbContext.CommissionRates.FirstOrDefault(rate => rate.RateId == rateId);

        if (commissionRateEntry == null)
            throw new NullReferenceException();

        if (commissionRateEntry.IsAccepted)
            throw new ArgumentException("Commission rate already exists and is accepted");
        
        commissionRateEntry.Rate = rate;
        
        await _postgresDbContext.SaveChangesAsync();
        
        commissionRateEntry = _postgresDbContext.CommissionRates.FirstOrDefault(rate => rate.RateId == rateId);

        return new CommissionRateDTO
        {
            RateId = commissionRateEntry.RateId,
            CreatedAt = commissionRateEntry.CreatedAt,
            CreatorId = commissionRateEntry.CreatorId,
            StoreId = commissionRateEntry.StoreId,
            Rate = commissionRateEntry.Rate,
            IsAccepted = commissionRateEntry.IsAccepted,
        };
    }

    public async Task<CommissionRateDTO?> DeleteCommissionRateAsync(Guid rateId)
    {
        throw new NotImplementedException();
    }
}