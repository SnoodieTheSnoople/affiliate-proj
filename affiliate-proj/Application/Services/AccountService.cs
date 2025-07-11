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

    private string GetUserEmailFromAcessToken()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value ??
               throw new UnauthorizedAccessException("User not authenticated.");
    }

    private bool CheckUserExists(Guid userId)
    {
        var user = _postgresDbContext.Users.Find(userId);
        if (user == null) return false;
        
        return true;
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
            Email = user.Email,
        };
    }

    /* Use only for testing. Do not use in production. */
    public async Task<UserDTO?> GetUserByEmailAsync(string email)
    {
        if (!GetUserEmailFromAcessToken().Equals(email)) throw new UnauthorizedAccessException("Email mismatch.");

        var user = await _postgresDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (user == null) return null;

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
        };
    }

    public async Task<UserDTO?> SetUserAsync(UserDTO userDto, Guid  userId)
    {
        if (!GetUserIdFromAccessToken().Equals(userId.ToString())) throw new UnauthorizedAccessException("User ID mismatch.");
        if (CheckUserExists(userId)) return null;

        var user = new User
        {
            UserId = userId,
            Username = userDto.Username,
            PhoneNumber = userDto.PhoneNumber,
            Email = userDto.Email,
            DeletedAt = null
        };

        _postgresDbContext.Users.Add(user);
        await _postgresDbContext.SaveChangesAsync();
        
        var returnUserEntry = await _postgresDbContext.Users.FindAsync(userId);
        return new UserDTO
        {
            UserId = returnUserEntry.UserId,
            Username = returnUserEntry.Username,
            PhoneNumber = returnUserEntry.PhoneNumber,
            Email = returnUserEntry.Email,
            CreatedAt = returnUserEntry.CreatedAt,
        };
    }

    public async Task<UserDTO?> UpdateEmailAsync(string email, Guid userId)
    {
        if (GetUserIdFromAccessToken() !=  userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!CheckUserExists(userId)) return null;

        var user = await _postgresDbContext.Users.FindAsync(userId);

        if (user == null) return null;
        
        user.Email = email;
        await _postgresDbContext.SaveChangesAsync();
        
        user = await _postgresDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
        };
    }

    public async Task<UserDTO?> UpdateUserNameAsync(string username, Guid userId)
    {
        if (GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!CheckUserExists(userId)) return null; 
        
        var user = await _postgresDbContext.Users.FindAsync(userId);
        if (user == null) return null;
        
        user.Username = username;
        await _postgresDbContext.SaveChangesAsync();
        
        user = await _postgresDbContext.Users.FirstOrDefaultAsync(user => user.Username == username);

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
        };
    }

    public async Task<UserDTO?> UpdatePhoneNumberAsync(string phoneNumber, Guid userId)
    {
        if (GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var user = await _postgresDbContext.Users.FindAsync(userId);
        if (user == null) return null;
        
        user.PhoneNumber = phoneNumber;
        await _postgresDbContext.SaveChangesAsync();
        
        user = await _postgresDbContext.Users.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber);

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
        };
    }

    /*
     * Update record and anonymise or replace PID with random, unrelated, data to remain compliant with GDPR.
     * TODO: Review in the future to ensure GDPR compliance.
     */
    public async Task<UserDTO?> DeleteUser(Guid userId)
    {
        if (GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var piiReplacement = Guid.NewGuid();
        var user = await _postgresDbContext.Users.FindAsync(userId);
        if (user == null) return null;
        
        user.Email = $"deleted_{piiReplacement}";
        user.Username = $"deleted_{piiReplacement}";
        user.PhoneNumber = $"deleted_{piiReplacement}";
        user.DeletedAt = DateTime.UtcNow;
        
        await _postgresDbContext.SaveChangesAsync();
        
        user = await _postgresDbContext.Users.FindAsync(userId);

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            CreatedAt = user.CreatedAt,
            Email = user.Email,
            DeletedAt = user.DeletedAt,
        };
    }

    /*
     *
     * Below is Creator related.
     * 
     */

    public async Task<CreatorDTO?> UpdateFirstNameAsync(string firstname, Guid userId)
    {
        if (GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        if (creator == null) return null;
        
        creator.Firstname = firstname;
        await _postgresDbContext.SaveChangesAsync();
        
        creator =  await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        return new CreatorDTO
        {
            CreatorId = creator.CreatorId,
            Firstname = creator.Firstname,
            Surname = creator.Surname,
            Dob = creator.Dob,
            StripeId = creator.StripeId,
            UserId = creator.UserId,
        };
    }

    public Task<CreatorDTO?> UpdateLastNameAsync(string lastname)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId)
    {
        if (GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        
        if (creator == null) return null;

        return new CreatorDTO
        {
            CreatorId = creator.CreatorId,
            CreatedAt = creator.CreatedAt,
            Firstname = creator.Firstname,
            Surname = creator.Surname,
            Dob = creator.Dob,
            StripeId = creator.StripeId,
            UserId = creator.UserId,
        };
    }

    public Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creator)
    {
        throw new NotImplementedException();
    }
}