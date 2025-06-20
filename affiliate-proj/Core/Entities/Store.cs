namespace affiliate_proj.Core.Entities;

public class Store
{
    public string StoreId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string StoreName { get; set; }
    public string ShopifyId { get; set; }
    public string ShopifyToken { get; set; }
    public string StoreUrl { get; set; }

    // TODO: Review entity on ER diagram.
    // TODO: Ensure complies with user flow for authentication utilising User entity.
}