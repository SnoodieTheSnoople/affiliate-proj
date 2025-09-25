using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Core.DTOs.Shopify.Products;

namespace affiliate_proj.Application.Services.Shopify.Data.Product;

public class ShopifyProductRepository : IShopifyProductRepository
{
    public async Task<ShopifyProductDTO> SetProductAsync(ShopifyProductDTO shopifyProductDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ShopifyProductDTO>> SetProductsListAsync(List<ShopifyProductDTO> shopifyProductDTOs)
    {
        throw new NotImplementedException();
    }
}