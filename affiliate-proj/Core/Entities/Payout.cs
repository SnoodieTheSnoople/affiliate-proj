namespace affiliate_proj.Core.Entities;

public class Payout
{
    public string PayoutId { get; set; }
    public string CreatorId { get; set; }
    public string BreakdownId  { get; set; }
    public string StripeReceipt { get; set; }
}