using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class AffiliateLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string LinkId { get; set; }
    
    [Required]
    public string CreatorId  { get; set; }
    
    [Required]
    public string StoreId  { get; set; }
    
    [Required]
    public string Link {  get; set; }
    
    [Required]
    public string RefParam { get; set; }
    
    [Required]
    public string ProductLink { get; set; }
    public int Clicks { get; set; }
    public DateTime CreatedAt { get; set; }
}