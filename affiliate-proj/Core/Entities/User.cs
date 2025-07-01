using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace affiliate_proj.Core.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public System.Guid UserId { get; set;  }
    
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    public DateTime DeletedAt { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
}