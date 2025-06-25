using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services;

public class AccountService : IAccountService
{
    private readonly SupabaseAccessor _supabaseAccessor;

    public AccountService(SupabaseAccessor supabaseAccessor)
    {
        _supabaseAccessor = supabaseAccessor;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = _supabaseAccessor.Users.AsNoTracking().ToList();
        return users;
    }

    public async Task<User> GetUserByIdAsync(System.Guid userId)
    {
        var user = await _supabaseAccessor.Users.FindAsync(userId);
        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        // Use for forgotten password?
        // Do not expose to any endpoint.
        var user = await _supabaseAccessor.Users.FindAsync(email);
        return user;
    }
}