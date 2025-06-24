using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class SupabaseAccessor(DbContextOptions<SupabaseAccessor> options) : DbContext (options)
{
    private readonly string _url;
    private readonly string _anonPublicKey;
    private readonly string _serviceRoleKey;
    
    public DbSet<User> Users { get; set; }

    // public SupabaseAccessor(string url,  string anonPublicKey, string serviceRoleKey)
    // {
    //     _url = url;
    //     _anonPublicKey = anonPublicKey;
    //     _serviceRoleKey = serviceRoleKey;
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
    }
    
    
    public void ShowKeys()
    {
        Console.WriteLine(_url);
        Console.WriteLine(_anonPublicKey);
        Console.WriteLine(_serviceRoleKey);
    }
}