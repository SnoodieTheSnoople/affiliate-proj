using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces;

public interface IAccountService
{ 
    Task<Core.DTOs.Account.UserDTO?> GetUserByIdAsync(Guid userId);
    Task<UserDTO?> GetUserByEmailAsync(string email);
    Task<UserDTO?> SetEmailAsync(string email, Guid userId);
    Task<UserDTO?> SetUserNameAsync(string username, Guid userId);
    Task<UserDTO?> SetPhoneNumberAsync(string phoneNumber, Guid userId);
    
    Task<CreatorDTO?> SetFirstNameAsync(string firstname);
    Task<CreatorDTO?> SetLastNameAsync(string lastname);
    Task<CreatorDTO?> SetDateOfBirthAsync(DateTime dateofbirth);
    Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId);
    Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creator);
}