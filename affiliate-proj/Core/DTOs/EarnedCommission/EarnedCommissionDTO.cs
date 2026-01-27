namespace affiliate_proj.Core.DTOs.EarnedCommission;

public class EarnedCommissionDTO
{
    public Guid CommissionId { get; set; }
    public Guid CreatorId  { get; set; }
    public Guid StoreId { get; set; }
    public Guid ConversionId  { get; set; }
    public decimal OrderCost   { get; set; }
    public decimal AmtEarned  { get; set; }
}