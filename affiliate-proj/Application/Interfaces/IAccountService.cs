﻿using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Interfaces;

public interface IAccountService
{ 
    Task<UserDTO?> GetUserByIdAsync(Guid userId);
    Task<UserDTO?> GetUserByEmailAsync(string email);
    Task<UserDTO?> SetUserAsync(UserDTO userDto, Guid  userId);
    Task<UserDTO?> UpdateEmailAsync(string email, Guid userId);
    Task<UserDTO?> UpdateUserNameAsync(string username, Guid userId);
    Task<UserDTO?> UpdatePhoneNumberAsync(string phoneNumber, Guid userId);
    Task<UserDTO?> DeleteUser(Guid userId);
    
    Task<CreatorDTO?> UpdateFirstNameAsync(string firstname, Guid userId);
    Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId);
    Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth, Guid userId);
    Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId);
    Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creator);
}