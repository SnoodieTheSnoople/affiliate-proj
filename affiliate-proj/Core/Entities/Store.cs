namespace affiliate_proj.Core.Entities;

public class Store
{
    public string StoreId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string StoreName { get; set; }
    public string ShopifyId { get; set; }
    public string ShopifyToken { get; set; }
    public string StoreUrl { get; set;  }
}