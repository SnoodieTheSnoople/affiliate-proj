using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IStoreService
{
    Task<Core.Entities.Store> GetAllStoresAsync();
}