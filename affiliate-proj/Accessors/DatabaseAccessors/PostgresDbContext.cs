using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class PostgresDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Creator> Creators { get; set; }
    
    public DbSet<Store> Stores { get; set; }
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.Property(user => user.UserId).HasColumnName("user_id").ValueGeneratedOnAdd();
            builder.Property(user => user.Username).HasColumnName("username");
            builder.Property(user => user.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(user => user.DeletedAt).HasColumnName("deleted_at");
            builder.Property(user => user.PhoneNumber).HasColumnName("phone_number");
            builder.Property(user => user.Email).HasColumnName("email");
        });

        modelBuilder.Entity<Creator>(builder =>
        {
            builder.ToTable("creators");
            builder.Property(creator => creator.CreatorId).HasColumnName("creator_id").ValueGeneratedOnAdd();
            builder.Property(creator => creator.UserId).HasColumnName("user_id");
            builder.Property(creator => creator.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(creator => creator.Firstname).HasColumnName("firstname");
            builder.Property(creator => creator.Surname).HasColumnName("surname");
            builder.Property(creator => creator.StripeId).HasColumnName("stripe_id");
            builder.Property(creator => creator.Dob).HasColumnName("dob");
        });

        modelBuilder.Entity<Store>(builder =>
        {
            builder.ToTable("stores");
            builder.Property(store => store.StoreId).HasColumnName("store_id").ValueGeneratedOnAdd();
            builder.Property(store => store.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(store => store.StoreName).HasColumnName("store_name");
            builder.Property(store => store.ShopifyId).HasColumnName("shopify_id");
            builder.Property(store => store.ShopifyToken).HasColumnName("shopify_token");
            builder.Property(store => store.StoreUrl).HasColumnName("store_url");
            builder.Property(store => store.ShopifyStoreName).HasColumnName("shopify_store_name");
            builder.Property(store => store.ShopifyOwnerName).HasColumnName("shopify_owner_name");
            builder.Property(store => store.ShopifyOwnerEmail).HasColumnName("shopify_owner_email");
            builder.Property(store => store.ShopifyOwnerPhone).HasColumnName("shopify_owner_phone");
            builder.Property(store => store.ShopifyCountry).HasColumnName("shopify_country");
            builder.Property(store => store.ShopifyGrantedScopes).HasColumnName("shopify_granted_scopes");
            builder.Property(store => store.UserId).HasColumnName("user_id");
            builder.Property(store => store.IsActive).HasColumnName("is_active");
            builder.Property(store => store.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<WebhookRegistrations>(builder =>
        {
            builder.ToTable("webhook_registrations");
            builder.Property(registration => registration.WebhookId).HasColumnName("webhook_id").ValueGeneratedOnAdd();
            builder.Property(registration => registration.CreatedAt).HasColumnName("created_at")
                .ValueGeneratedOnAddOrUpdate();
            builder.Property(registration => registration.StoreUrl).HasColumnName("store_url");
            builder.Property(registration => registration.ShopifyWebhookId).HasColumnName("shopify_webhook_id");
            builder.Property(registration => registration.Topic).HasColumnName("topic");
            builder.Property(registration => registration.Format).HasColumnName("format");
            builder.Property(registration => registration.RegisteredAt).HasColumnName("registered_at");
            builder.Property(registration => registration.StoreId).HasColumnName("store_id");
        });
    }

}