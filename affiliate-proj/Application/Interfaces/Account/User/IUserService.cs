using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.User;

public interface IUserService
{
    Task<UserDTO?> SetUserAsync(UserDTO userDto, Guid userId);
    Task<UserDTO?> GetUserByUserIdAsync(Guid userId);
    Task<UserDTO?> DeleteUser(Guid userId, Guid piiReplacementId);
    Task<UserDTO?> UpdateEmailAsync(string email, Guid userId);
    Task<UserDTO?> UpdateUserNameAsync(string username, Guid userId);
    Task<UserDTO?> UpdatePhoneNumberAsync(string phoneNumber, Guid userId);
}