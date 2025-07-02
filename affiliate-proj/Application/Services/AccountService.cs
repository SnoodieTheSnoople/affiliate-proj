using System.Security.Claims;
using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services;

public class AccountService : IAccountService
{
    private readonly SupabaseAccessor _supabaseAccessor;
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(SupabaseAccessor supabaseAccessor,  PostgresDbContext postgresDbContext,  
        IHttpContextAccessor httpContextAccessor)
    {
        _supabaseAccessor = supabaseAccessor;
        _postgresDbContext = postgresDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserIdFromAccessToken()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value ??
               throw new UnauthorizedAccessException("User not authenticated.");
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
    {
        if (GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var user = await _postgresDbContext.Users.FindAsync(userId);
        
        if (user == null) return null;

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
        };
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