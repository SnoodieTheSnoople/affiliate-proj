using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services;

public class AccountService : IAccountService
{
    private readonly SupabaseAccessor _supabaseAccessor;
    private readonly PostgresDbContext _postgresDbContext;

    public AccountService(SupabaseAccessor supabaseAccessor,  PostgresDbContext postgresDbContext)
    {
        _supabaseAccessor = supabaseAccessor;
        _postgresDbContext = postgresDbContext;
    }

    public Task<User> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserNameAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetPhoneNumberAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetUserNameAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetPhoneNumberAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetFirstNameAsync(string firstname)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetLastNameAsync(string lastname)
    {
        throw new NotImplementedException();
    }

    public Task<User> SetDateOfBirthAsync(DateTime dateofbirth)
    {
        throw new NotImplementedException();
    }
}