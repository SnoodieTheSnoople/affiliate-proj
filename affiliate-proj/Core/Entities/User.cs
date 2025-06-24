using System.ComponentModel.DataAnnotations;

namespace affiliate_proj.Core.Entities;

public class User
{
    [Required]
    public string Uuid { get; set;  }
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}