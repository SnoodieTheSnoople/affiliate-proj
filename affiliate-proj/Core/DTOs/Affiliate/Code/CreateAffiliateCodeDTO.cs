namespace affiliate_proj.Core.DTOs.Affiliate.Code;

public class CreateAffiliateCodeDTO
{
    public Guid CreatorId { get; set; }
    public Guid StoreId { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public int ValidFor { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
}