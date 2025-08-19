using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Store;

public interface IStoreService
{
    Task<CreateStoreDTO?> SetShopifyStoreAsync(CreateStoreDTO storeDto);
    Task<List<StoreDTO>> GetAllStoresAsync();
    Task<StoreDTO> GetStoreByIdAsync(Guid storeId);
    Task<Core.Entities.Store> GetStoreDetailsByIdAsync(Guid storeId);
    Task<CreateStoreDTO?> SyncStoreAsync(Guid storeId);
    Task<Core.Entities.Store?> GetStoreDetailsByShopifyStoreIdAsync(long shopifyStoreId);
    Task<Core.Entities.Store?> UpdateStoreDetailsAsync(Core.Entities.Store store);
}