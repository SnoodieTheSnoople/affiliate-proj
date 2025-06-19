namespace affiliate_proj.Core.Entities;

public class User
{
    public string uuid { get; }
    public DateTime created_at { get; }
    public string username { get; set; }
    public string password { get; set; }
}