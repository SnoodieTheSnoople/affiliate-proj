namespace affiliate_proj.Core.DTOs.Shopify.Products;

public class ShopifyProductDTO
{
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public string ShopifyProductId { get; set; }
    public string Title { get; set; }
    public string Handle { get; set; }
    public bool HasOnlyDefaultVariant { get; set; }
    public string OnlineStoreUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime SyncedAt { get; set; }
}