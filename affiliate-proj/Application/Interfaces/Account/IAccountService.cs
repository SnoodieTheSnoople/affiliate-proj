using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces;

public interface IAccountService
{ 
    Task<UserDTO?> DeleteUser(Guid userId);
    Task<ProfileDTO?> DeleteUserProfileAsync(Guid userId);
}