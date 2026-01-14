using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class Conversion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ConversionId { get; set; }
    
    [Required]
    public Guid StoreId  { get; set; }
    
    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; }
    public string Link  { get; set; }
    public int Clicks { get; set; }
    public string Code  { get; set; }
    
    [Required]
    public long ShopifyOrderId { get; set; }
    
    [Required]
    public decimal OrderCost  { get; set; }
    
    [Required]
    public string Currency { get; set; }
    public string OrderStatus { get; set; }
    public DateTime OrderCreated  { get; set; }
    public string LandingSite  { get; set; }
    public string LandingSiteRef  { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { set; get; }
}