using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
    {
        
    }
    
}