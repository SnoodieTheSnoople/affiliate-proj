using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces;

public interface IAccountService
{ 
    Task<Core.DTOs.Account.UserDTO?> GetUserByIdAsync(Guid userId);
    Task<UserDTO?> GetUserByEmailAsync(string email);
    
    Task<UserDTO> GetUserNameAsync(System.Guid userId);
    Task<UserDTO> GetPhoneNumberAsync(System.Guid userId);
    
    Task<UserDTO> SetUserNameAsync(User userDto);
    Task<UserDTO> SetPhoneNumberAsync(User userDto);
    
    Task<UserDTO> SetFirstNameAsync(string firstname);
    Task<UserDTO> SetLastNameAsync(string lastname);
    Task<UserDTO> SetDateOfBirthAsync(DateTime dateofbirth);
    Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId);
    Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creator);
}