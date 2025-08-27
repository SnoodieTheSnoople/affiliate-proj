using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Account;

public interface IAccountService
{ 
    Task<UserDTO?> DeleteUser(Guid userId);
    Task<ProfileDTO?> DeleteUserProfileAsync(Guid userId);
}