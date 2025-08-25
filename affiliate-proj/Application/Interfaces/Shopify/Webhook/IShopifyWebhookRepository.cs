using affiliate_proj.Core.DTOs.Account;
using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookRepository
{
    Task<CreateWebhookRegistrationDTO> SetShopifyWebhook(CreateWebhookRegistrationDTO registration);
    Task<CreateWebhookRegistrationDTO?> DeleteShopifyWebhook(Shop shop);
}