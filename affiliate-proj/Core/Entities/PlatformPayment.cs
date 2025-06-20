namespace affiliate_proj.Core.Entities;

public class PlatformPayment
{
    public string PaymentId { get; set; }
    public string StoreId { get; set; }
    public float AmtPaid { get; set; }
    public string StripeReceipt { get; set; }
}