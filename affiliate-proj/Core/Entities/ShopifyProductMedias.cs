using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class ShopifyProductMedias
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MediaId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [ForeignKey(nameof(ProductId))]
    public ShopifyProducts Product { get; set; }
    
    [Required]
    public string ShopifyProductId { get; set; }
    
    [Required]
    public string Alt { get; set; }
    
    [Required]
    public string MediaType { get; set; }
    
    [Required]
    public string ImageUrl  { get; set; }
    
    [Required]
    public int Width { get; set; }
    
    [Required]
    public int Height { get; set; }
    
    public DateTime CreatedAt { get; set; }
}