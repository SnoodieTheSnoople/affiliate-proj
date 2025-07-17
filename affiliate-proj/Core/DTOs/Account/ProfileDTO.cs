namespace affiliate_proj.Core.DTOs.Account;

public class ProfileDTO
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Email { get; set; }
    public Guid CreatorId { get; set; }
    public string Firstname  { get; set; }
    public string Surname { get; set; }
    public DateOnly Dob { get; set; }
    public string? StripeId  { get; set; }
}