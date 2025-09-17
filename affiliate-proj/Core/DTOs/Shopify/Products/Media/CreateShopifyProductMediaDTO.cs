namespace affiliate_proj.Core.DTOs.Shopify.Products.Media;

public class CreateShopifyProductMediaDTO
{
    public Guid ProductId { get; set; }
    public string Alt { get; set; }
    public string MediaType { get; set; }
    public string ImageUrl  { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; }
}