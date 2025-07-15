using System.Security.Claims;
using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Creator;
using affiliate_proj.Application.Interfaces.User;
using affiliate_proj.Core.DTOs.Account;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services;

public class AccountService : IAccountService
{
    private readonly SupabaseAccessor _supabaseAccessor;
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountHelper _accountHelper;
    private readonly IUserService _userService;
    private readonly ICreatorService _creatorService;

    public AccountService(SupabaseAccessor supabaseAccessor,  PostgresDbContext postgresDbContext,  
        IHttpContextAccessor httpContextAccessor, IAccountHelper accountHelper, IUserService userService, 
        ICreatorService creatorService)
    {
        _supabaseAccessor = supabaseAccessor;
        _postgresDbContext = postgresDbContext;
        _httpContextAccessor = httpContextAccessor;
        _accountHelper = accountHelper;
        _userService = userService;
        _creatorService = creatorService;
    }

    private string GetUserEmailFromAcessToken()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value ??
               throw new UnauthorizedAccessException("User not authenticated.");
    }
    

    /*
     * Update record and anonymise or replace PID with random, unrelated, data to remain compliant with GDPR.
     * TODO: Review in the future to ensure GDPR compliance.
     */
    public async Task<UserDTO?> DeleteUser(Guid userId)
    {
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
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
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
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

    public async Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId)
    {
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        if (creator == null) return null;
        
        creator.Surname = surname;
        await _postgresDbContext.SaveChangesAsync();
        
        creator = await  _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
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

    public async Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth, Guid  userId)
    {
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");

        var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        if (creator == null) return null;
        
        creator.Dob = dateofbirth;
        await _postgresDbContext.SaveChangesAsync();
        
        creator = await  _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
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

    public async Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId)
    {
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        
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

    public async Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creatorDto, Guid userId)
    {
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");

        var newCreatorRecord = new Core.Entities.Creator
        {
            Firstname = creatorDto.Firstname,
            Surname = creatorDto.Surname,
            Dob = creatorDto.Dob,
            StripeId = creatorDto.StripeId,
            UserId = creatorDto.UserId,
        };
        
        var checkCreator =  await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        if (checkCreator != null) return null;
        
        await _postgresDbContext.Creators.AddAsync(newCreatorRecord);
        await _postgresDbContext.SaveChangesAsync();
        
        checkCreator = await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        return new CreatorDTO
        {
            CreatorId = checkCreator.CreatorId,
            CreatedAt = checkCreator.CreatedAt,
            Firstname = checkCreator.Firstname,
            Surname = checkCreator.Surname,
            Dob = checkCreator.Dob,
            StripeId = checkCreator.StripeId,
            UserId = checkCreator.UserId,
        };
    }

    public async Task<CreatorDTO?> DeleteCreator(Guid userId, Guid piiReplacement)
    {
        if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId)) throw new UnauthorizedAccessException("User ID mismatch.");
        
        var creator =  await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
        if (creator == null) return null;

        creator.Firstname = $"deleted_{piiReplacement}";
        creator.Surname = $"deleted_{piiReplacement}";
        creator.Dob = new DateTime(1970, 1, 1);
        await _postgresDbContext.SaveChangesAsync();

        creator = await _postgresDbContext.Creators.FirstOrDefaultAsync( creator => creator.UserId == userId);
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
}