using affiliate_proj.Accessors.DatabaseAccessors;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Core.DTOs.Shopify.Products;
using affiliate_proj.Core.DTOs.Shopify.Products.Media;
using affiliate_proj.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace affiliate_proj.Application.Services.Shopify.Data.Product;

public class ShopifyProductRepository : IShopifyProductRepository
{
    private readonly PostgresDbContext _dbContext;

    public ShopifyProductRepository(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
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
        
        var existingProduct = await _dbContext.ShopifyProducts
            .AsNoTracking()
            .Where(product => product.StoreId == storeId && 
                              checkShopifyProductId.Contains(product.ShopifyProductId))
            .Select(product => product.ShopifyProductId)
            .ToListAsync();
        
        // TODO: Consider update if there is an existing entry
        
        var newProductList = shopifyProductsList
            .Where(product => !existingProduct.Contains(product.ShopifyProductId))
            .ToList();
        
        await _dbContext.ShopifyProducts.AddRangeAsync(newProductList);
        await _dbContext.SaveChangesAsync();
        
        var returnProducts = await _dbContext.ShopifyProducts
            .AsNoTracking()
            .Where(product => product.StoreId == storeId && 
                              checkShopifyProductId.Contains(product.ShopifyProductId))
            .ToListAsync();

        return returnProducts.Select(toDto => new ShopifyProductDTO
        {
            ProductId = toDto.ProductId,
            StoreId = toDto.StoreId,
            ShopifyProductId = toDto.ShopifyProductId,
            Title = toDto.Title,
            Handle = toDto.Handle,
            HasOnlyDefaultVariant = toDto.HasOnlyDefaultVariant,
            OnlineStoreUrl = toDto.OnlineStoreUrl,
            CreatedAt = toDto.CreatedAt,
            SyncedAt = toDto.SyncedAt,
            UpdatedAt = toDto.UpdatedAt
        }).ToList();
    }

    public async Task<List<ShopifyProductDTO>> UpdateProductsListAsync(List<ShopifyProducts> shopifyProductsList)
    {
        throw new NotImplementedException();
    }
}