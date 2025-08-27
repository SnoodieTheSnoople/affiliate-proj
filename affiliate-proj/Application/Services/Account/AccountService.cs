using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Account.User;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.Account;

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

    public async Task<ProfileDTO?> DeleteUserProfileAsync(Guid userId)
    {
        try
        {
            if (_accountHelper.GetUserIdFromAccessToken() != userId.ToString()) return null;

            var piiReplacement = Guid.NewGuid();
            var user = await _userService.DeleteUserAsync(userId, piiReplacement);
            if (user == null) throw new NullReferenceException("User failed deletion.");
            
            var creator = await _creatorService.DeleteCreatorAsync(userId, piiReplacement);
            if (creator == null) throw new NullReferenceException("Creator failed deletion.");

            return new ProfileDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                DeletedAt = user.DeletedAt,
                Email = user.Email,
                CreatorId = creator.CreatorId,
                Firstname = creator.Firstname,
                Surname = creator.Surname,
                Dob = creator.Dob,
                StripeId = creator.StripeId,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}