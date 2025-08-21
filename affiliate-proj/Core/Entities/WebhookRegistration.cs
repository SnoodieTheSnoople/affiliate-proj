using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class WebhookRegistration
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid WebhookId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string StoreUrl { get; set; }
    
    [Required]
    public long ShopifyWebhookId { get; set; }
    
    [Required]
    public string Topic { get; set; }
    
    [Required]
    public string Format { get; set; }
    
    [Required]
    public DateTime RegisteredAt { get; set; }
    
    [Required]
    public Guid StoreId { get; set; }
    
    [ForeignKey("StoreId")]
    public Store Store { get; set; }
}