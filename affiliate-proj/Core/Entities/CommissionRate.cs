using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class CommissionRate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RateId {  get; set; }
    
    [Required]
    public Guid CreatorId { get; set; }
    
    [ForeignKey(nameof(CreatorId))]
    public Creator Creator { get; set; }
    
    [Required]
    public Guid StoreId { get; set; }
    
    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; }
    
    [Required]
    public float Rate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public bool IsAccepted { get; set; }
}