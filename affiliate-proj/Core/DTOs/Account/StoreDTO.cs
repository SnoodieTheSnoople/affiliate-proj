namespace affiliate_proj.Core.DTOs.Account;

public class StoreDTO
{
    public Guid StoreId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? StoreName { get; set; }
    public long ShopifyId { get; set; }
    public string ShopifyToken { get; set; }
    public string StoreUrl { get; set; }
    public string ShopifyStoreName { get; set; }
    public string ShopifyOwnerName { get; set; }
    public string ShopifyOwnerEmail { get; set; }
    public string? ShopifyOwnerPhone { get; set; }
    public string ShopifyCountry { get; set; }
}