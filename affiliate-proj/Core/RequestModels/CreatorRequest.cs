namespace affiliate_proj.Core.RequestModels;

public class CreatorRequest
{
    public Guid CreatorId { get; set; }
    
    public string? Firstname  { get; set; }
    
    public string? Surname { get; set; }
    
    public DateOnly? Dob { get; set; }
    
    public string? StripeId  { get; set; }
    
    public Guid UserId { get; set; }
}
