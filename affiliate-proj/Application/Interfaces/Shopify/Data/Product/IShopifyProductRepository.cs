using affiliate_proj.Core.DTOs.Shopify.Products;

namespace affiliate_proj.Application.Interfaces.Shopify.Data.Product;

public interface IShopifyProductRepository
{
    Task<ShopifyProductDTO> SetProductAsync(ShopifyProductDTO shopifyProductDTO);
    Task<List<ShopifyProductDTO>> SetProductsListAsync(List<ShopifyProductDTO> shopifyProductDTOs, Guid storeId);
}