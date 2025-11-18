using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Account.Affiliate.Code;
using affiliate_proj.Core.DTOs.Affiliate.Code;

namespace affiliate_proj.Application.Services.Account.Affiliate.Code;

public class AffiliateCodeRepository : IAffiliateCodeRepository
{
    private readonly PostgresDbContext _dbContext;

    public AffiliateCodeRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AffiliateCodeDTO?> GetAffiliateCodeAsync(string code)
    {
        throw new NotImplementedException();
    }
}