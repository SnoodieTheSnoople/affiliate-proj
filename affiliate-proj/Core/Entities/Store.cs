using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class Store
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Guid StoreId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string StoreName { get; set; }
    
    [Required]
    public string ShopifyId { get; set; }
    
    [Required]
    public string ShopifyToken { get; set; }
    
    [Required]
    public string StoreUrl { get; set; }

    // TODO: Review entity on ER diagram.
    // TODO: Ensure complies with user flow for authentication utilising User entity.
}