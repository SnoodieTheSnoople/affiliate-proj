using affiliate_proj.Core.DTOs.Account;
using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookRepository
{
    Task<CreateWebhookRegistrationDTO> SetShopifyWebhookAsync(CreateWebhookRegistrationDTO registration);
    Task<bool> DeleteShopifyWebhook(Shop shop);
}