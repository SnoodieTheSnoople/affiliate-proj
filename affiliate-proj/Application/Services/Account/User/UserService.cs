using affiliate_proj.Application.Interfaces.User;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.User;

public class UserService : IUserService
{
    public Task<UserDTO?> SetUserAsync(UserDTO user, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> GetUserByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> DeleteUser(Guid userId)
    {
        throw new NotImplementedException();
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