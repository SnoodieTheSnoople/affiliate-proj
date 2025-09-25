using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Core.DTOs.Shopify.Products;

namespace affiliate_proj.Application.Services.Shopify.Data.Product;

public class ShopifyProductRepository : IShopifyProductRepository
{
    private readonly PostgresDbContext _context;

    public ShopifyProductRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public async Task<ShopifyProductDTO> SetProductAsync(ShopifyProductDTO shopifyProductDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ShopifyProductDTO>> SetProductsListAsync(List<ShopifyProductDTO> shopifyProductDTOs)
    {
        throw new NotImplementedException();
    }
}