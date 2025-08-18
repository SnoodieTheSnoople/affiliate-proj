using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Store;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Store;

public class StoreService : IStoreService
{
    private readonly PostgresDbContext _postgresDbContext;

    public StoreService(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }
    
    public async Task<List<Core.Entities.Store>> GetAllStoresAsync()
    {
        var stores = await _postgresDbContext.Stores.ToListAsync();
        return stores;
        throw new NotImplementedException();
    }
}