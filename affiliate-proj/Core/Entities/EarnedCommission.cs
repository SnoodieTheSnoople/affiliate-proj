using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class EarnedCommission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CommissionId { get; set; }
    
    [Required]
    public Guid CreatorId  { get; set; }
    
    [ForeignKey(nameof(CreatorId))]
    public Creator Creator { get; set; }
    
    [Required]
    public Guid StoreId { get; set; }
    
    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; }
    
    [Required]
    public Guid ConversionId  { get; set; }
    
    [ForeignKey(nameof(ConversionId))]
    public Conversion Conversion { get; set; }
    
    [Required]
    public decimal OrderCost   { get; set; }
    
    [Required]
    public decimal AmtEarned  { get; set; }
}