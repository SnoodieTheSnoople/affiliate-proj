namespace affiliate_proj.Core.Entities;

public class Creator
{
    private string _creatorId { get; set; }
    private DateTime _createdAt { get; set; }
    private string _firstname  { get; set; }
    private string _lastname { get; set; }
    private DateOnly _dob { get; set; }
    private string _stripeId  { get; set; }
    private string _userId  { get; set; }
}