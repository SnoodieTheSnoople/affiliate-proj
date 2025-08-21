using affiliate_proj.Core.DTOs.Account;
using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IStoreService
{
    Task<CreateStoreDTO?> SetShopifyStoreAsync(CreateStoreDTO storeDto);
    Task<List<StoreDTO>> GetAllStoresAsync();
    Task<StoreDTO> GetStoreByIdAsync(Guid storeId);
    Task<List<StoreDTO>> GetAllActiveStoresAsync();
    Task<Core.Entities.Store> GetStoreDetailsByIdAsync(Guid storeId);
    Task<CreateStoreDTO?> SyncStoreAsync(Guid storeId);
    Task<CreateStoreDTO?> UpdateStoreNameAsync(string storeName, Guid storeId);
}