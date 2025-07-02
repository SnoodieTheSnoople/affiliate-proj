using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class PostgresDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Creator> Creators { get; set; }
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.Property(user => user.UserId).HasColumnName("user_id");
            builder.Property(user => user.Username).HasColumnName("username");
            builder.Property(user => user.CreatedAt).HasColumnName("created_at");
            builder.Property(user => user.DeletedAt).HasColumnName("deleted_at");
            builder.Property(user => user.PhoneNumber).HasColumnName("phone_number");
        });

        modelBuilder.Entity<Creator>(builder =>
        {
            builder.ToTable("Creator");
            builder.Property(creator => creator.CreatorId).HasColumnName("creator_id");
            builder.Property(creator => creator.UserId).HasColumnName("user_id");
            builder.Property(creator => creator.CreatedAt).HasColumnName("created_at");
            builder.Property(creator => creator.Firstname).HasColumnName("firstname");
            builder.Property(creator => creator.Lastname).HasColumnName("surname");
            builder.Property(creator => creator.StripeId).HasColumnName("stripe_id");
            builder.Property(creator => creator.Dob).HasColumnName("dob");
        });
    }

}