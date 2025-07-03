using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

[Table("users")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set;  }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
    
    [Required]
    public string Email { get; set; }
}