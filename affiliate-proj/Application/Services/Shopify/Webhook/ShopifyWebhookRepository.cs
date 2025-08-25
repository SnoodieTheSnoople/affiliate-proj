using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;
using ShopifySharp;

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

        newRegistration = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(
            r => r.ShopifyWebhookId == registration.ShopifyWebhookId);
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

    public async Task<bool> DeleteShopifyWebhook(Shop shop)
    {
        var id = shop.Id;
        var store = await _dbContext.Stores.FirstOrDefaultAsync(s => s.ShopifyId == id);
        
        var webhook = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(r => r.StoreId == store.StoreId);
        
        _dbContext.WebhookRegistrations.Remove(webhook);
        await _dbContext.SaveChangesAsync();
        
        webhook = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(r => r.StoreId == store.StoreId);
        
        return webhook == null;
    }
}