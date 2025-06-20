namespace affiliate_proj.Core.Entities;

public class AffiliateLink
{
    public string LinkId { get; set; }
    public string CreatorId  { get; set; }
    public string StoreId  { get; set; }
    public string Link {  get; set; }
    public string RefParam { get; set; }
    public string ProductLink { get; set; }
    public int Clicks { get; set; }
}