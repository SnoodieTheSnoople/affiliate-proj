namespace affiliate_proj.Core.DTOs.Rates;

public class CreateCommissionRateDTO
{
    public string CreatorId { get; set; }
    public string StoreId { get; set; }
    public float Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAccepted { get; set; }
}