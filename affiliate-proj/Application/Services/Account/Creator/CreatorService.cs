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

            if (!CheckCreatorExists(userId)) return null;

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

    public Task<CreatorDTO?> GetCreatorByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> DeleteCreatorAsync(Guid userId, Guid piiReplacementId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateFirstNameAsync(string firstName, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateSurnameAsync(string surname, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<CreatorDTO?> UpdateDateOfBirthAsync(DateTime dateofbirth, Guid userId)
    {
        throw new NotImplementedException();
    }
}