using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Creator;
using affiliate_proj.Core.DTOs.Account;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Creator;

public class CreatorService : ICreatorService
{
    private readonly PostgresDbContext _postgresDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAccountHelper _accountHelper;

    public CreatorService(PostgresDbContext postgresDbContext, IHttpContextAccessor httpContextAccessor,
        IAccountHelper accountHelper)
    {
        _postgresDbContext = postgresDbContext;
        _httpContextAccessor = httpContextAccessor;
        _accountHelper = accountHelper;
    }

    private bool CheckCreatorExists(Guid userId)
    {
        var creator = _postgresDbContext.Creators.FirstOrDefault(c => c.UserId == userId);
        if (creator == null) return false;
        
        return true;
    }

    private bool ValidateUser(Guid userId)
    {
        if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
            throw new UnauthorizedAccessException("User ID mismatch.");
        if (!_accountHelper.CheckUserExists(userId))
            throw new UnauthorizedAccessException("User not found.");
        if (!CheckCreatorExists(userId))
            throw new UnauthorizedAccessException("Creator not found.");
        
        return true;
    }

    public async Task<CreatorDTO?> SetCreatorAsync(CreatorDTO creatorDto, Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch.");

            if (!_accountHelper.CheckUserExists(userId))
                throw new UnauthorizedAccessException("User not found.");

            var newCreatorRecord = new Core.Entities.Creator
            {
                Firstname = creatorDto.Firstname,
                Surname = creatorDto.Surname,
                Dob = creatorDto.Dob,
                StripeId = creatorDto.StripeId,
                UserId = creatorDto.UserId,
            };

            if (CheckCreatorExists(userId)) return null;

            await _postgresDbContext.Creators.AddAsync(newCreatorRecord);
            await _postgresDbContext.SaveChangesAsync();

            var checkCreatorExists =
                await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);

            return new CreatorDTO
            {
                CreatorId = checkCreatorExists.CreatorId,
                CreatedAt = checkCreatorExists.CreatedAt,
                Firstname = checkCreatorExists.Firstname,
                Surname = checkCreatorExists.Surname,
                Dob = checkCreatorExists.Dob,
                StripeId = checkCreatorExists.StripeId,
                UserId = checkCreatorExists.UserId
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new  UnauthorizedAccessException("User ID mismatch.");
            
            var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            if (creator == null) return null;

            return new CreatorDTO
            {
                CreatorId = creator.CreatorId,
                CreatedAt = creator.CreatedAt,
                Firstname = creator.Firstname,
                Surname = creator.Surname,
                Dob = creator.Dob,
                StripeId = creator.StripeId,
                UserId = creator.UserId
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<CreatorDTO?> DeleteCreatorAsync(Guid userId, Guid piiReplacementId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new  UnauthorizedAccessException("User ID mismatch.");
            
            if (!_accountHelper.CheckUserExists(userId))
                throw new  UnauthorizedAccessException("User not found.");
            
            if (!CheckCreatorExists(userId))
                throw new  UnauthorizedAccessException("Creator not found.");
            
            var creator = _postgresDbContext.Creators.FirstOrDefault(creator => creator.UserId == userId);
            if (creator == null) return null;
            
            creator.Firstname = $"deleted_{piiReplacementId}";
            creator.Surname = $"deleted_{piiReplacementId}";
            creator.Dob = new DateOnly(1970,1,1);
            await _postgresDbContext.SaveChangesAsync();
            
            creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            return new CreatorDTO
            {
                CreatorId = creator.CreatorId,
                CreatedAt = creator.CreatedAt,
                Firstname = creator.Firstname,
                Surname = creator.Surname,
                Dob = creator.Dob,
                StripeId = creator.StripeId,
                UserId = creator.UserId
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<CreatorDTO?> UpdateFirstNameAsync(string firstName, Guid userId)
    {
        try
        {
            if (!_accountHelper.GetUserIdFromAccessToken().Equals(userId.ToString()))
                throw new UnauthorizedAccessException("User ID mismatch.");

            if (!_accountHelper.CheckUserExists(userId))
                throw new UnauthorizedAccessException("User ID not found.");

            if (!CheckCreatorExists(userId))
                throw new UnauthorizedAccessException("Creator not found.");

            var creator = _postgresDbContext.Creators.FirstOrDefault(creator => creator.UserId == userId);
            if (creator == null) return null;

            creator.Firstname = firstName;
            await _postgresDbContext.SaveChangesAsync();

            creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            return new CreatorDTO
            {
                CreatorId = creator.CreatorId,
                CreatedAt = creator.CreatedAt,
                Firstname = creator.Firstname,
                Surname = creator.Surname,
                Dob = creator.Dob,
                StripeId = creator.StripeId,
                UserId = creator.UserId
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId)
    {
        try
        {
            if (!ValidateUser(userId)) return null;
            
            var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            if (creator == null) return null;
            
            creator.Surname = surname;
            await _postgresDbContext.SaveChangesAsync();
            
            creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            return new CreatorDTO
            {
                CreatorId = creator.CreatorId,
                CreatedAt = creator.CreatedAt,
                Dob = creator.Dob,
                Firstname = creator.Firstname,
                Surname = creator.Surname,
                StripeId = creator.StripeId,
                UserId = creator.UserId
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<CreatorDTO?> UpdateDateOfBirthAsync(DateOnly dateofbirth, Guid userId)
    {
        try
        {
            if (!ValidateUser(userId)) return null;
            
            var creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            if (creator == null) return null;
            
            creator.Dob = dateofbirth;
            await _postgresDbContext.SaveChangesAsync();
            
            creator = await _postgresDbContext.Creators.FirstOrDefaultAsync(creator => creator.UserId == userId);
            return new CreatorDTO
            {
                CreatorId = creator.CreatorId,
                CreatedAt = creator.CreatedAt,
                Firstname = creator.Firstname,
                Surname = creator.Surname,
                Dob = creator.Dob,
                StripeId = creator.StripeId,
                UserId = creator.UserId
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}