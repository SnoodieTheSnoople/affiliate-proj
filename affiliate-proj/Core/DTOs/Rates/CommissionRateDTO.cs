namespace affiliate_proj.Core.DTOs.Rates;

public class CommissionRateDTO
{
    public Guid RateId {  get; set; }
    public Guid CreatorId { get; set; }
    public Guid StoreId { get; set; }
    public float Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAccepted { get; set; }
}