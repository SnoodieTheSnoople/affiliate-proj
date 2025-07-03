using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

[Table("creators")]
public class Creator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Firstname  { get; set; }
    
    [Required]
    public string Lastname { get; set; }
    
    [Required]
    public DateTime Dob { get; set; }
    
    public string? StripeId  { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User  { get; set; }
}