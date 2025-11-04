namespace affiliate_proj.Core.DTOs.Affiliate.Link;

public class AffiliateLinkDTO
{
    public string LinkId { get; set; }
    public Guid CreatorId  { get; set; }
    public Guid StoreId  { get; set; }
    public string Link {  get; set; }
    public string RefParam { get; set; }
    public string ProductLink { get; set; }
    public int Clicks { get; set; }
    public DateTime CreatedAt { get; set; }
}