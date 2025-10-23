using ShopifySharp.GraphQL;

namespace affiliate_proj.Core.DataTypes.GraphQL;

public class CustomListProductsResult
{
    public CustomProductsConnection Products { get; set; }
}

public class CustomProductsConnection
{
    public PageInfo PageInfo { get; set; }
    public List<CustomProductNode> Nodes { get; set; }
}

public class CustomProductNode
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Handle { get; set; }
    public bool HasOnlyDefaultVariant { get; set; }
    public string OnlineStoreUrl { get; set; }
    public string OnlineStorePreviewUrl { get; set; }
    public bool PublishedOnCurrentPublication { get; set; }
    public CustomResourcePublicationsV2 ResourcePublicationsV2 { get; set; }
    public CustomMediaConnection Media { get; set; }
}

public class CustomMediaConnection
{
    public List<CustomMediaNode> Nodes { get; set; }
}

public class CustomMediaNode
{
    public string Id { get; set; }
    public string MediaContentType { get; set; }
    public string Alt { get; set; }
    public CustomPreview Preview { get; set; }
}

public class CustomPreview
{
    public CustomImage Image { get; set; }
}

public class CustomImage
{
    public string Id { get; set; }
    public string AltText { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
    public string Url { get; set; }
}

public class CustomResourcePublicationsV2
{
    public List<CustomResourcePublicationsV2Node>  Nodes { get; set; }
}

public class CustomResourcePublicationsV2Node
{
    public bool IsPublished  { get; set; }
    public CustomPublication Publication { get; set; }
}

public class CustomPublication
{
}