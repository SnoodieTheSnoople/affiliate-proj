namespace affiliate_proj.Core.DTOs.Rates;

public class CreateCommissionRateDTO
{
    public Guid CreatorId { get; set; }
    public Guid StoreId { get; set; }
    public float Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAccepted { get; set; }
}