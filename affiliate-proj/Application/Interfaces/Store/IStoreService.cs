using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IStoreService
{
    Task<List<StoreDTO>> GetAllStoresAsync();
    Task<StoreDTO> GetStoreByIdAsync(Guid storeId);
}