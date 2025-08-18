using affiliate_proj.Application.Interfaces.Store;

namespace affiliate_proj.Application.Services.Store;

public class StoreService : IStoreService
{
    public Task<Core.Entities.Store> GetAllStoresAsync()
    {
        throw new NotImplementedException();
    }
}