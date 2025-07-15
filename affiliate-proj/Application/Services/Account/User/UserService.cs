using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.User;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.User;

public class UserService : IUserService
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountHelper _accountHelper;

    public UserService(PostgresDbContext postgresDbContext, IHttpContextAccessor httpContextAccessor, 
        IAccountHelper accountHelper)
    {
        _postgresDbContext = postgresDbContext;
        _httpContextAccessor = httpContextAccessor;
        _accountHelper = accountHelper;
    }
    
    public Task<UserDTO?> SetUserAsync(UserDTO user, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO?> GetUserByUserIdAsync(Guid userId)
    {
        try
        {
            if (String.IsNullOrEmpty(_accountHelper.GetUserIdFromAccessToken()))
                throw new UnauthorizedAccessException("User not authenticated.");

            var user = await _postgresDbContext.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserDTO?> DeleteUser(Guid userId, Guid piiReplacementId)
    {
        if (String.IsNullOrEmpty(_accountHelper.GetUserIdFromAccessToken()))
            throw new UnauthorizedAccessException("User not authenticated.");
        
        if (!_accountHelper.CheckUserExists(userId))
            throw new UnauthorizedAccessException("User not found.");
        
        var user = await _postgresDbContext.Users.FindAsync(userId);
        if (user == null) return null;

        user.Email = $"deleted_{piiReplacementId}";
        user.Username = $"deleted_{piiReplacementId}";
        user.PhoneNumber = $"deleted_{piiReplacementId}";
        user.DeletedAt = DateTime.UtcNow;
        await _postgresDbContext.SaveChangesAsync();
        
        user = await _postgresDbContext.Users.FindAsync(userId);

        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            DeletedAt = user.DeletedAt,
        };
    }

    public Task<UserDTO?> UpdateEmailAsync(string email, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> UpdateUserNameAsync(string username, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> UpdatePhoneNumberAsync(string phoneNumber, Guid userId)
    {
        throw new NotImplementedException();
    }
}