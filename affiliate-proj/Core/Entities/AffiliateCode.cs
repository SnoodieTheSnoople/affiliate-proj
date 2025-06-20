namespace affiliate_proj.Core.Entities;

public class AffiliateCode
{
    public string CodeId { get; set; }
    public string CreatorId { get; set; }
    public string StoreId { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public int ValidFor { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
}