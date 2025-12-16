namespace affiliate_proj.Core.Entities;

public class Conversion
{
    public Guid ConversionId { get; set; }
    public Guid StoreId  { get; set; }
    public string Link  { get; set; }
    public int Clicks { get; set; }
    public string Code  { get; set; }
    public string ShopifyOrderId { get; set; }
    public float OrderCost  { get; set; }
    public string Currency { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderCreated  { get; set; }
    public string LandingSite  { get; set; }
    public string LandingSiteRef  { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { set; get; }
}