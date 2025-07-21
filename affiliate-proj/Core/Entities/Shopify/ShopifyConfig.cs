namespace affiliate_proj.Core.Entities.Shopify;

public class ShopifyConfig
{
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string AppUrl { get; set; }
    public List<string> PermissionScopes { get; set; } = new();
}