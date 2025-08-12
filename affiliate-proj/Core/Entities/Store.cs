using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class Store
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid StoreId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? StoreName { get; set; }
    
    [Required]
    public long ShopifyId { get; set; }
    
    [Required]
    public string ShopifyToken { get; set; }
    
    [Required]
    public string StoreUrl { get; set; }
    
    [Required]
    public string ShopifyStoreName { get; set; }
    
    [Required]
    public string ShopifyOwnerName { get; set; }
    
    [Required]
    public string ShopifyOwnerEmail { get; set; }
    
    public string ShopifyOwnerPhone { get; set; }
    
    [Required]
    public string ShopifyCountry { get; set; }
}