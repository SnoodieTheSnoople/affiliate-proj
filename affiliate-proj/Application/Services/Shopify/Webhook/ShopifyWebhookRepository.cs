using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookRepository : IShopifyWebhookRepository
{
    private readonly PostgresDbContext _dbContext;

    public ShopifyWebhookRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CreateWebhookRegistrationDTO> SetShopifyWebhook(CreateWebhookRegistrationDTO registration)
    {
        var newRegistration = new WebhookRegistrations
        {
            StoreUrl = registration.StoreUrl,
            ShopifyWebhookId = registration.ShopifyWebhookId,
            Topic = registration.Topic,
            Format = registration.Format,
            RegisteredAt = registration.RegisteredAt,
            StoreId = registration.StoreId,
        };
        
        await _dbContext.WebhookRegistrations.AddAsync(newRegistration);
        await _dbContext.SaveChangesAsync();

        newRegistration = await _dbContext.WebhookRegistrations.FindAsync(registration.ShopifyWebhookId);
        return new CreateWebhookRegistrationDTO
        {
            WebhookId = newRegistration.WebhookId,
            CreatedAt = newRegistration.CreatedAt,
            StoreUrl = newRegistration.StoreUrl,
            ShopifyWebhookId = newRegistration.ShopifyWebhookId,
            Topic = newRegistration.Topic,
            Format = newRegistration.Format,
            RegisteredAt = newRegistration.RegisteredAt,
            StoreId = newRegistration.StoreId,
        };
    }
}