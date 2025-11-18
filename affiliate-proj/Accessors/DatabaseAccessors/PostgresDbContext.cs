using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class PostgresDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Creator> Creators { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<WebhookRegistrations> WebhookRegistrations { get; set; }
    public DbSet<CommissionRate> CommissionRates { get; set; }
    public DbSet<ShopifyProducts> ShopifyProducts { get; set; }
    public DbSet<ShopifyProductMedias> ShopifyProductMedias { get; set; }
    public DbSet<AffiliateLink> AffiliateLinks { get; set; }
    public DbSet<AffiliateCode> AffiliateCodes { get; set; }
    
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
            builder.Property(user => user.IsStoreOwner).HasColumnName("is_store_owner");
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

        modelBuilder.Entity<CommissionRate>( builder =>
        {
            builder.ToTable("commission_rates");
            builder.Property(rate => rate.RateId).HasColumnName("rate_id").ValueGeneratedOnAdd();
            builder.Property(rate => rate.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(rate => rate.StoreId).HasColumnName("store_id");
            builder.Property(rate => rate.CreatorId).HasColumnName("creator_id");
            builder.Property(rate => rate.Rate).HasColumnName("commission_rate");
            builder.Property(rate => rate.IsAccepted).HasColumnName("is_accepted");
        });

        modelBuilder.Entity<ShopifyProducts>(builder =>
        {
            builder.ToTable("shopify_products");
            builder.Property(product => product.ProductId).HasColumnName("product_id");
            builder.Property(product => product.StoreId).HasColumnName("store_id");
            builder.Property(product => product.ShopifyProductId).HasColumnName("shopify_product_id");
            builder.Property(product => product.Title).HasColumnName("title");
            builder.Property(product => product.Handle).HasColumnName("handle");
            builder.Property(product => product.HasOnlyDefaultVariant).HasColumnName("has_only_default_variant");
            builder.Property(product => product.OnlineStoreUrl).HasColumnName("online_store_url");
            builder.Property(product => product.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(product => product.UpdatedAt).HasColumnName("updated_at");
            builder.Property(product => product.SyncedAt).HasColumnName("synced_at");
        });

        modelBuilder.Entity<ShopifyProductMedias>(builder =>
        {
            builder.ToTable("shopify_product_medias");
            builder.Property(media => media.MediaId).HasColumnName("media_id");
            builder.Property(media => media.ProductId).HasColumnName("product_id");
            builder.Property(media => media.ShopifyProductId).HasColumnName("shopify_product_id");
            builder.Property(media => media.Alt).HasColumnName("alt");
            builder.Property(media => media.MediaType).HasColumnName("media_type");
            builder.Property(media => media.ImageUrl).HasColumnName("image_url");
            builder.Property(media => media.Width).HasColumnName("width");
            builder.Property(media => media.Height).HasColumnName("height");
            builder.Property(media => media.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
        });

        modelBuilder.Entity<AffiliateLink>(builder =>
        {
            builder.ToTable("affiliate_links");
            builder.Property(link => link.LinkId).HasColumnName("link_id");
            builder.Property(link => link.CreatorId).HasColumnName("creator_id");
            builder.Property(link => link.StoreId).HasColumnName("store_id");
            builder.Property(link => link.Link).HasColumnName("affiliate_link");
            builder.Property(link => link.RefParam).HasColumnName("ref_param");
            builder.Property(link => link.ProductLink).HasColumnName("product_link");
            builder.Property(link => link.Clicks).HasColumnName("clicks");
            builder.Property(link => link.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(link => link.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<AffiliateCode>(builder =>
        {
            builder.ToTable("affiliate_codes");
            builder.Property(code => code.CodeId).HasColumnName("code_id");
            builder.Property(code => code.CreatorId).HasColumnName("creator_id");
            builder.Property(code => code.StoreId).HasColumnName("store_id");
            builder.Property(code => code.Code).HasColumnName("affiliate_code");
            builder.Property(code => code.IsActive).HasColumnName("is_active");
            builder.Property(code => code.ValidFor).HasColumnName("valid_for");
            builder.Property(code => code.ExpiryDate).HasColumnName("expiry_date");
            builder.Property(code => code.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAddOrUpdate();
            builder.Property(code => code.ProductLink).HasColumnName("product_link");
        });
    }

}