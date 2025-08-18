namespace affiliate_proj.Core.DTOs.Account;

public class StoreDTO
{
    public Guid StoreId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? StoreName { get; set; }
    public string StoreUrl { get; set; }
    public string ShopifyCountry { get; set; }
}