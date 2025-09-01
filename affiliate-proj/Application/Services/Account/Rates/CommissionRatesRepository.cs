using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Core.DTOs.Rates;

namespace affiliate_proj.Application.Services.Account.Rates;

public class CommissionRatesRepository
{
    private readonly PostgresDbContext _postgresDbContext;

    public CommissionRatesRepository(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<CreateCommissionRateDTO> SetCommissionRateAsync(CommissionRateDTO commissionRate)
    {
        throw new NotImplementedException();
    }
}