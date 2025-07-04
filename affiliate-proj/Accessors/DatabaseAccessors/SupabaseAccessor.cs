using Supabase.Gotrue;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class SupabaseAccessor
{
    private readonly string _url;
    private readonly string _anonPublicKey;
    private readonly string _serviceRoleKey;
    private readonly Supabase.Client _client;
    
    
    // public DbSet<User> Users { get; set; }

    // public SupabaseAccessor(DbContextOptions<SupabaseAccessor> options) : base(options)
    // {
    //     
    // }

    public SupabaseAccessor(string url,  string anonPublicKey, string serviceRoleKey)
    {
        _url = url;
        _anonPublicKey = anonPublicKey;
        _serviceRoleKey = serviceRoleKey;
    }

    public SupabaseAccessor(Supabase.Client client)
    {
        _client = client;
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //     // modelBuilder.Entity<User>().ToTable("users");
    //     // // modelBuilder.Entity<User>().Property(x => x.UserId).HasColumnName("user_id");
    //     // modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
    //     // // modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
    //     // // modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
    //     // // modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
    //
    //     modelBuilder.Entity<User>(builder =>
    //     {
    //         builder.ToTable("users");
    //         builder.Property(user => user.Id).HasColumnName("user_id");
    //         builder.Property(user => user.CreatedAt).HasColumnName("created_at");
    //         builder.Property(user => user.UserName).HasColumnName("username");
    //         builder.Property(user => user.Email).HasColumnName("email");
    //         // Named "password" for simplicity. ALl passwords are hashed and not plaintext.
    //         builder.Property(user => user.PasswordHash).HasColumnName("password");
    //     });
    // }
    
    public void ShowKeys()
    {
        Console.WriteLine(_url);
        Console.WriteLine(_anonPublicKey);
        Console.WriteLine(_serviceRoleKey);
    }

    public User? GetCurrentUser()
    {
        return _client.Auth.CurrentUser; 
    }
}