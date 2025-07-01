using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

[Table("creators")]
public class Creator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Firstname  { get; set; }
    
    [Required]
    public string Lastname { get; set; }
    
    [Required]
    public DateOnly Dob { get; set; }
    
    public string StripeId  { get; set; }
    
    [ForeignKey("UserId")]
    public User User  { get; set; }
}