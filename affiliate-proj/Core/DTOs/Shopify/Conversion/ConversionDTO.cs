namespace affiliate_proj.Core.DTOs.Shopify.Conversion;

public class ConversionDTO
{
    public Guid ConversionId { get; set; }
    public Guid StoreId  { get; set; }
    public string Link  { get; set; }
    public int Clicks { get; set; }
    public string Code  { get; set; }
    public long ShopifyOrderId { get; set; }
    public decimal OrderCost  { get; set; }
    public string Currency { get; set; }
    public string OrderStatus { get; set; }
    public DateTimeOffset OrderCreated  { get; set; }
    public string LandingSite  { get; set; }
    public string LandingSiteRef  { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { set; get; }
}