namespace affiliate_proj.Application.Interfaces.Account;

public interface IAccountHelper
{
    string GetUserIdFromAccessToken();
    bool CheckUserExists(Guid userId);
    Task<bool> CheckUserIsStoreOwnerAsync(Guid userId);
}