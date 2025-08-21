namespace affiliate_proj.Core.DTOs.Account;

public class CreateWebhookRegistration
{
    public Guid WebhookId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string StoreUrl { get; set; }
    public long ShopifyWebhookId { get; set; }
    public string Topic { get; set; }
    public string Format { get; set; }
    public DateTime RegisteredAt { get; set; }
    public Guid StoreId { get; set; }
}