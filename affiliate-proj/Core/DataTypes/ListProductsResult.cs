namespace affiliate_proj.Core.DataTypes;

public record ListProductsResult
{
    public required ShopifySharp.GraphQL.ProductConnection Products { get; set; }
}