using affiliate_proj.Core.DTOs.Account;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookRepository
{
    Task<CreateWebhookRegistrationDTO> SetShopifyWebhook(CreateWebhookRegistrationDTO registration);
}