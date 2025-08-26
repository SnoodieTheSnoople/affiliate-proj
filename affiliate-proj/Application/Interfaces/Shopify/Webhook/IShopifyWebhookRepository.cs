using affiliate_proj.Core.DTOs.Account;
using ShopifySharp;

namespace affiliate_proj.Application.Interfaces.Shopify.Webhook;

public interface IShopifyWebhookRepository
{
    Task<CreateWebhookRegistrationDTO> SetShopifyWebhookAsync(CreateWebhookRegistrationDTO registration);
    Task<bool> DeleteShopifyWebhooksAsync(Shop shop);
    Task<bool> DeleteShopifyWebhookAsync(long shopifyWebhookId);
    Task<bool> DeleteShopifyWebhooksAsync(Guid storeId);
}