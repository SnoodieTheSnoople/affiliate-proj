using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Services.Shopify.Webhook;

public class ShopifyWebhookRepository : IShopifyWebhookRepository
{
    public Task<CreateWebhookRegistrationDTO> SetShopifyWebhook(CreateWebhookRegistrationDTO registration)
    {
        throw new NotImplementedException();
    }
}