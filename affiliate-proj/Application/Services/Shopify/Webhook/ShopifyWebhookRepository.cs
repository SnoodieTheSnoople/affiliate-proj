using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookRepository : IShopifyWebhookRepository
{
    private readonly PostgresDbContext _dbContext;

    public ShopifyWebhookRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<CreateWebhookRegistrationDTO> SetShopifyWebhook(CreateWebhookRegistrationDTO registration)
    {
        throw new NotImplementedException();
    }
}