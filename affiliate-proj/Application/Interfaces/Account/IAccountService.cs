using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces;

public interface IAccountService
{ 
    Task<UserDTO?> DeleteUser(Guid userId);
    
    Task<CreatorDTO?> UpdateFirstNameAsync(string firstname, Guid userId);
    Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId);
    Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth, Guid userId);
    Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId);
    Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creatorDto, Guid userId);
    Task<CreatorDTO?> DeleteCreator(Guid userId, Guid piiReplacement);
}