using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class AffiliateCode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CodeId { get; set; }
    
    [Required]
    public string CreatorId { get; set; }
    
    [ForeignKey(nameof(CreatorId))]
    public Creator Creator { get; set; }
    
    [Required]
    public string StoreId { get; set; }
    
    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; }
    
    [Required]
    public string Code { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    [Required]
    public int ValidFor { get; set; }
    
    [Required]
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
}