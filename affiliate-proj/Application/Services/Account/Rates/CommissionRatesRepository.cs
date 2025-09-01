using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Core.DTOs.Rates;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Account.Rates;

public class CommissionRatesRepository
{
    private readonly PostgresDbContext _postgresDbContext;

    public CommissionRatesRepository(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<CreateCommissionRateDTO> SetCommissionRateAsync(CommissionRate commissionRate)
    {
        var checkRateExists = _postgresDbContext.CommissionRates
            .Where(rate => rate.CreatorId == commissionRate.CreatorId)
            .Where(rate => rate.StoreId == commissionRate.StoreId)
            .FirstOrDefaultAsync();
        
        if (checkRateExists != null)
            throw new Exception("Commission rate already exists");
        
        throw new NotImplementedException();
    }
}