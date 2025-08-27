using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.User;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.Account.User;

public class UserService : IUserService
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IAccountHelper _accountHelper;

    public UserService(PostgresDbContext postgresDbContext, 
        IAccountHelper accountHelper)
    {
        _postgresDbContext = postgresDbContext;
        _accountHelper = accountHelper;
    }
    
    public async Task<UserDTO?> SetUserAsync(UserDTO userDto, Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch");

            if (_accountHelper.CheckUserExists(userId)) return null;

            var user = new Core.Entities.User
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
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<UserDTO?> GetUserByUserIdAsync(Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch.");

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

    public async Task<UserDTO?> DeleteUserAsync(Guid userId, Guid piiReplacementId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch.");

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
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<UserDTO?> UpdateEmailAsync(string email, Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch.");

            if (!_accountHelper.CheckUserExists(userId))
                throw new UnauthorizedAccessException("User not found.");
            
            var user = await _postgresDbContext.Users.FindAsync(userId);
            if (user == null) return null;
            /*TODO: Consider a way to differentiate between internal error, or issue with no user found.*/
            
            user.Email = email;
            await _postgresDbContext.SaveChangesAsync();
            
            user = await _postgresDbContext.Users.FindAsync(userId);

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<UserDTO?> UpdateUserNameAsync(string username, Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch.");
            
            if (!_accountHelper.CheckUserExists(userId)) 
                throw new UnauthorizedAccessException("User not found.");
            
            var user = await _postgresDbContext.Users.FindAsync(userId);
            if (user == null) return null;
            
            user.Username = username;
            await _postgresDbContext.SaveChangesAsync();
            
            user = await _postgresDbContext.Users.FindAsync(userId);
            
            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<UserDTO?> UpdatePhoneNumberAsync(string phoneNumber, Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new  UnauthorizedAccessException("User ID mismatch.");
            
            if (!_accountHelper.CheckUserExists(userId))
                throw new  UnauthorizedAccessException("User not found.");
            
            var user = await _postgresDbContext.Users.FindAsync(userId);
            if (user == null) return null;
            
            user.PhoneNumber = phoneNumber;
            await _postgresDbContext.SaveChangesAsync();
            
            user = await _postgresDbContext.Users.FindAsync(userId);

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}