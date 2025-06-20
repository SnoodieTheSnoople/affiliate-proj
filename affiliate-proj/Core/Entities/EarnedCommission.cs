namespace affiliate_proj.Core.Entities;

public class EarnedCommission
{
    public string CommissionId { get; set; }
    public string CreatorId  { get; set; }
    public string StoreId { get; set; }
    public string ConversionId  { get; set; }
    public float OrderCost   { get; set; }
    public float AmtEarned  { get; set; }
}