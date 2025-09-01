using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class CommissionRate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string RateId {  get; set; }
    
    [Required]
    public string CreatorId { get; set; }
    
    [ForeignKey(nameof(CreatorId))]
    public Creator Creator { get; set; }
    
    [Required]
    public string StoreId { get; set; }
    
    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; }
    
    [Required]
    public float Rate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public bool IsAccepted { get; set; }
}