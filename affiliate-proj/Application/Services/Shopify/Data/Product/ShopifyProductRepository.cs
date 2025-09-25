using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Core.DTOs.Shopify.Products;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<ShopifyProductDTO>> SetProductsListAsync(List<ShopifyProductDTO> shopifyProductDTOs, 
        Guid storeId)
    {
        var shopifyProductsList = shopifyProductDTOs.Select(dto => new ShopifyProducts
        {
            ProductId = dto.ProductId,
            StoreId = dto.StoreId,
            ShopifyProductId = dto.ShopifyProductId,
            Title = dto.Title,
            Handle = dto.Handle,
            HasOnlyDefaultVariant = dto.HasOnlyDefaultVariant,
            OnlineStoreUrl = dto.OnlineStoreUrl
        }).ToList();
        
        var checkShopifyProductId = shopifyProductsList.Select(product => product.ShopifyProductId).ToHashSet();
        
        var existingProduct = await _context.ShopifyProducts
            .Where(product => product.StoreId == storeId && 
                              checkShopifyProductId.Contains(product.ShopifyProductId))
            .Select(product => product.ShopifyProductId)
            .ToListAsync();
        
        // TODO: Consider update if there is an existing entry
        
        var newProductList = shopifyProductsList
            .Where(product => !existingProduct.Contains(product.ShopifyProductId))
            .ToList();
        
        await _context.ShopifyProducts.AddRangeAsync(newProductList);
        await _context.SaveChangesAsync();
        
        throw new NotImplementedException();
    }
}