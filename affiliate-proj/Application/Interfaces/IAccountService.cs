using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces;

public interface IAccountService
{ 
    string? GetUserByIdAsync(Guid userId);
    Task<User> GetUserByEmailAsync(string email);
    
    Task<User> GetUserNameAsync(System.Guid userId);
    Task<User> GetPhoneNumberAsync(System.Guid userId);
    
    Task<User> SetUserNameAsync(User user);
    Task<User> SetPhoneNumberAsync(User user);
    
    Task<User> SetFirstNameAsync(string firstname);
    Task<User> SetLastNameAsync(string lastname);
    Task<User> SetDateOfBirthAsync(DateTime dateofbirth);
    
}