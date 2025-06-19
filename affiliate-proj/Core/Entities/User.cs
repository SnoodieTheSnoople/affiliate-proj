namespace affiliate_proj.Core.Entities;

public class User
{
    public string uuid { get; set;  }
    public DateTime created_at { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}