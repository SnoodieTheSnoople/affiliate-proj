namespace affiliate_proj.Core.Entities;

public class PayoutBreakdown
{
    public string BreakdownId { get; set; }
    public string CreatorId  { get; set; }
    public string StoreId  { get; set; }
    public float TotalCommission { get; set; }
    public string Currency  { get; set; }
    public DateTime GeneratedAt { get; set; }
}