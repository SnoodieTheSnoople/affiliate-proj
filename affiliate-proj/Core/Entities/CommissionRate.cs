namespace affiliate_proj.Core.Entities;

public class CommissionRate
{
    public string RateId {  get; set; }
    public string CreatorId { get; set; }
    public string StoreId { get; set; }
    public float Rate { get; set; }
}