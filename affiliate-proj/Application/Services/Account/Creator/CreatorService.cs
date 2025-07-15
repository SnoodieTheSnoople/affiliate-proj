using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Creator;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.Creator;

public class CreatorService : ICreatorService
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountHelper _accountHelper;

    public CreatorService(PostgresDbContext postgresDbContext, IHttpContextAccessor httpContextAccessor,
        IAccountHelper accountHelper)
    {
        _postgresDbContext = postgresDbContext;
        _httpContextAccessor = httpContextAccessor;
        _accountHelper = accountHelper;
    }

    public Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creator, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> DeleteCreatorAsync(Guid userId, Guid piiReplacementId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateFirstNameAsync(string firstName, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth, Guid userId)
    {
        throw new NotImplementedException();
    }
}