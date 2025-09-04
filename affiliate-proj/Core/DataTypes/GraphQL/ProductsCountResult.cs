namespace affiliate_proj.Core.DataTypes.GraphQL;

public class ProductsCountResult
{
    public ProductsCount ProductsCount { get; set; }
}

public class ProductsCount
{
    public int count { get; set; }
}