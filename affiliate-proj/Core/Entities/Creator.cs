namespace affiliate_proj.Core.Entities;

public class Creator
{
    private string creator_id { get; set; }
    private DateTime created_at { get; set; }
    private string firstname  { get; set; }
    private string lastname { get; set; }
    private DateOnly dob { get; set; }
    private string stripe_id  { get; set; }
    private string user_id  { get; set; }
}