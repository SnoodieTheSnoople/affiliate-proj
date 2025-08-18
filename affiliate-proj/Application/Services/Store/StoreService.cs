using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Store;

namespace affiliate_proj.Application.Services.Store;

public class StoreService : IStoreService
{
    private readonly PostgresDbContext _postgresDbContext;

    public StoreService(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }
    
    public Task<Core.Entities.Store> GetAllStoresAsync()
    {
        throw new NotImplementedException();
    }
}