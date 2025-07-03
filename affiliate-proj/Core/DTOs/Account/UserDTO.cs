namespace affiliate_proj.Core.DTOs.Account;

public class UserDTO
{
    
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}