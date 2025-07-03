namespace affiliate_proj.Core.DTOs.Account;

public class CreatorDTO
{
    public string CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Firstname  { get; set; }
    
    public string Lastname { get; set; }
    
    public DateTime Dob { get; set; }
    
    public string? StripeId  { get; set; }
    
    public Guid UserId { get; set; }
}