using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class ShopifyProducts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductId { get; set; }
    
    [Required]
    public Guid StoreId { get; set; }
    
    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; }
    
    [Required]
    public string ShopifyProductId { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Handle { get; set; }
    
    [Required]
    public string HasOnlyDefaultVariant { get; set; }
    
    [Required]
    public string OnlineStoreUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime SyncedAt { get; set; }
}