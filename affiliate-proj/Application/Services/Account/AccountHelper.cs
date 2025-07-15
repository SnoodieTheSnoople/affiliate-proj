using System.Security.Claims;
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
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || String.IsNullOrEmpty(userId)) 
            throw new UnauthorizedAccessException("User not authenticated.");
        
        return userId;
    }

    public bool CheckUserExists(Guid userId)
    {
        throw new NotImplementedException();
    }
}