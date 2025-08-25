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
    public async Task<CreateWebhookRegistrationDTO> SetShopifyWebhookAsync(CreateWebhookRegistrationDTO registration)
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

    public async Task<bool> DeleteShopifyWebhookAsync(Shop shop)
    {
        var id = shop.Id;
        var store = await _dbContext.Stores.FirstOrDefaultAsync(s => s.ShopifyId == id);

        if (store == null) 
            return false;
        
        // var webhook = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(r => r.StoreId == store.StoreId);
        
        // if (webhook == null) 
        //     return false;

        // _dbContext.WebhookRegistrations.Remove(webhook);
        // await _dbContext.SaveChangesAsync();
        //
        // webhook = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(r => r.StoreId == store.StoreId);
        //
        // return webhook == null;
        
        var webhooks = await _dbContext.WebhookRegistrations
            .Where(r => r.StoreId == store.StoreId).ToListAsync();

        if (!webhooks.Any())
            return false;
        
        _dbContext.WebhookRegistrations.RemoveRange(webhooks);
        await _dbContext.SaveChangesAsync();
        
        var checkWebhooksExist = await _dbContext.WebhookRegistrations.AnyAsync(r => r.StoreId == store.StoreId);
        
        return !checkWebhooksExist;
    }

    public async Task<bool> DeleteShopifyWebhookAsync(long webhookId)
    {
        var webhook = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(r => r.ShopifyWebhookId == webhookId);
        _dbContext.WebhookRegistrations.Remove(webhook);
        await _dbContext.SaveChangesAsync();
        
        webhook = await _dbContext.WebhookRegistrations.FirstOrDefaultAsync(r => r.ShopifyWebhookId == webhookId);
        
        return webhook == null;
    }
}