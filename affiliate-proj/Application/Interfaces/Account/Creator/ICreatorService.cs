using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Creator;

public interface ICreatorService
{
    Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creatorDto, Guid userId);
    Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId);
    Task<CreatorDTO?> DeleteCreatorAsync (Guid userId, Guid piiReplacementId);
    Task<CreatorDTO?> UpdateFirstNameAsync(string firstName, Guid userId);
    Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId);
    Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth, Guid userId);
}