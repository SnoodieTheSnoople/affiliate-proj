namespace affiliate_proj.Application.Interfaces;

public interface IAccountHelper
{
    string GetUserIdFromAccessToken();
    bool CheckUserExists(Guid userId);
}