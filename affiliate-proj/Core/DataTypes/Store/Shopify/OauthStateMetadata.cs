namespace affiliate_proj.Core.DataTypes.Store.Shopify;

public class OauthStateMetadata
{
    public string State { get; set; }
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }
}