namespace affiliate_proj.Core.Entities;

public class Creator
{
    public string CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string _Firstname  { get; set; }
    public string Lastname { get; set; }
    public DateOnly Dob { get; set; }
    public string StripeId  { get; set; }
    public string UserId  { get; set; }
}