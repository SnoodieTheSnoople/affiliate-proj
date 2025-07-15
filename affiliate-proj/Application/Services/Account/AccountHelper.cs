using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;

namespace affiliate_proj.Application.Services;

public class AccountHelper : IAccountHelper
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountHelper(PostgresDbContext postgresDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _postgresDbContext = postgresDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserIdFromAccessToken()
    {
        throw new NotImplementedException();
    }

    public bool CheckUserExists(Guid userId)
    {
        throw new NotImplementedException();
    }
}