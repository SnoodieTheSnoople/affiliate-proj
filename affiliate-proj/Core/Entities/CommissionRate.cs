namespace affiliate_proj.Core.Entities;

public class CommissionRate
{
    public string RateId {  get; set; }
    public string CreatorUid { get; set; }
    public string StoreUid { get; set; }
    public float Rate { get; set; }
}