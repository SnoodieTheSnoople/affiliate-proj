namespace affiliate_proj.Core.DTOs.Account;

public class CreatorDTO
{
    public Guid CreatorId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Firstname  { get; set; }
    
    public string Surname { get; set; }
    
    public DateOnly Dob { get; set; }
    
    public string? StripeId  { get; set; }
    
    public Guid UserId { get; set; }
}