using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IStoreService
{
    Task<List<Core.Entities.Store>> GetAllStoresAsync();
    Task<Core.Entities.Store> GetStoreByIdAsync(Guid storeId);
}