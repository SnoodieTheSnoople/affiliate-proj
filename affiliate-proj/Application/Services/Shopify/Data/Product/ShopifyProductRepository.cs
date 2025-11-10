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
        if (shopifyProductDTOs == null)
            throw new ArgumentNullException();
        
        if (storeId == Guid.Empty)
            throw new ArgumentNullException();
        
        if (shopifyProductDTOs.Count == 0)
            return new List<ShopifyProductDTO>();
        
        var shopifyProductsList = shopifyProductDTOs.Select(dto => new ShopifyProducts
        {
            ProductId = dto.ProductId,
            StoreId = storeId,
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
        
        var listToUpdate = shopifyProductsList.Where(elements => existingProduct.Contains(elements.ShopifyProductId)).ToList();
        var updatedList = await UpdateProductsListAsync(listToUpdate, storeId);
        
        
        var newProductList = shopifyProductsList
            .Where(product => !existingProduct.Contains(product.ShopifyProductId))
            .ToList();
        
        var newProductListIds = newProductList.Select(id => id.ProductId).ToList();
        
        await _dbContext.ShopifyProducts.AddRangeAsync(newProductList);
        await _dbContext.SaveChangesAsync();
        
        var newProducts = await _dbContext.ShopifyProducts
            .AsNoTracking()
            .Where(product => product.StoreId == storeId && 
                              newProductListIds.Contains(product.ProductId))
            .ToListAsync();


        var returnProducts = newProducts.Select(toDto => new ShopifyProductDTO
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
        
        returnProducts.AddRange(updatedList);

        return returnProducts;
    }

    public async Task<List<ShopifyProductDTO>> UpdateProductsListAsync(List<ShopifyProducts> shopifyProductsList,
        Guid storeId)
    {
        if (shopifyProductsList == null)
           throw new ArgumentNullException();
        
        if (shopifyProductsList.Count == 0)
            return new List<ShopifyProductDTO>();
        
        var updateDateTime = DateTime.UtcNow;

        var productsDictByShopifyId = shopifyProductsList
            .GroupBy(product => product.ShopifyProductId)
            .Select(group => group.First())
            .ToDictionary(product => product.ShopifyProductId, product => product);
        
        var productsListShopifyIds = productsDictByShopifyId.Keys.ToList();

        var existingProducts = await _dbContext.ShopifyProducts
            .Where(product => product.StoreId == storeId && 
                              productsListShopifyIds.Contains(product.ShopifyProductId))
            .ToListAsync();

        foreach (var product in existingProducts)
        {
            var getProductFromDict = productsDictByShopifyId[product.ShopifyProductId];
            
            product.Title = getProductFromDict.Title;
            product.Handle = getProductFromDict.Handle;
            product.HasOnlyDefaultVariant = getProductFromDict.HasOnlyDefaultVariant;
            product.OnlineStoreUrl = getProductFromDict.OnlineStoreUrl;
            product.UpdatedAt = updateDateTime;
        }
        
        await _dbContext.SaveChangesAsync();

        var getProducts = await _dbContext.ShopifyProducts
            .AsNoTracking()
            .Where(product => product.StoreId == storeId &&
                              productsListShopifyIds.Contains(product.ShopifyProductId))
            .ToListAsync();
        
        return getProducts.Select(toDto => new ShopifyProductDTO
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

    public async Task<List<ShopifyProductMediaDTO>> SetShopifyProductMediaListAsync(
        List<CreateShopifyProductMediaDTO> shopifyProductMediaDTOs)
    {
        if (shopifyProductMediaDTOs == null) throw new  ArgumentNullException(nameof(shopifyProductMediaDTOs));
        if (shopifyProductMediaDTOs.Count == 0) return new List<ShopifyProductMediaDTO>();

        var convertDtos = shopifyProductMediaDTOs.Select(dto => new ShopifyProductMedias
        {
            ProductId = dto.ProductId,
            ShopifyProductId = dto.ShopifyProductId,
            Alt = dto.Alt,
            MediaType = dto.MediaType,
            ImageUrl = dto.ImageUrl,
            Width = dto.Width,
            Height = dto.Height,
        }).ToList();
        
        // Assuming that only 1 image/media is stored
        
        var setOfProductIdsToCheck = convertDtos.Select(dto => dto.ProductId).ToHashSet();
        var existingRecords = await _dbContext.ShopifyProductMedias
            .AsNoTracking()
            .Where(media => setOfProductIdsToCheck.Contains(media.ProductId))
            .Select(media => media.ProductId)
            .ToListAsync();
        
        var listToUpdate = convertDtos.Where(media => 
            existingRecords.Contains(media.ProductId)).ToList();
        var updatedList = await UpdateShopifyProductMediaListAsync(listToUpdate);
        
        var newMediaToAddList = convertDtos.Where(
            media => !existingRecords.Contains(media.ProductId)).ToList();

        var newMediaProductIds = newMediaToAddList.Select(id => id.ProductId).ToList();

        await _dbContext.ShopifyProductMedias.AddRangeAsync(newMediaToAddList);
        await _dbContext.SaveChangesAsync();

        var newMedias = await _dbContext.ShopifyProductMedias
            .AsNoTracking()
            .Where(media => newMediaProductIds.Contains(media.ProductId))
            .ToListAsync();

        var returnMedias = newMedias.Select(toDto => new ShopifyProductMediaDTO
        {
            MediaId = toDto.MediaId,
            ProductId = toDto.ProductId,
            ShopifyProductId = toDto.ShopifyProductId,
            Alt = toDto.Alt,
            MediaType = toDto.MediaType,
            ImageUrl = toDto.ImageUrl,
            Width = toDto.Width,
            Height = toDto.Height,
            CreatedAt = toDto.CreatedAt
        }).ToList();
        
        returnMedias.AddRange(updatedList);
        
        return returnMedias;
    }

    public async Task<List<ShopifyProductMediaDTO>> UpdateShopifyProductMediaListAsync(
        List<ShopifyProductMedias> shopifyProductMediasList)
    {
        if (shopifyProductMediasList == null)
            throw new ArgumentException();
        
        if (shopifyProductMediasList.Count == 0)
            return new List<ShopifyProductMediaDTO>();
        
        var productMediaDictByShopifyId = shopifyProductMediasList
            .GroupBy(media => media.ShopifyProductId)
            .Select(group => group.First())
            .ToDictionary(media => media.ShopifyProductId);
        
        var mediaListShopifyIds = productMediaDictByShopifyId.Keys.ToList();
        
        var existingProducts = await _dbContext.ShopifyProductMedias
            .Where(media => mediaListShopifyIds.Contains(media.ShopifyProductId))
            .ToListAsync();

        foreach (var media in existingProducts)
        {
            var getMediaFromDict = productMediaDictByShopifyId[media.ShopifyProductId];
            
            media.Alt = getMediaFromDict.Alt;
            media.MediaType = getMediaFromDict.MediaType;
            media.ImageUrl = getMediaFromDict.ImageUrl;
            media.Width = getMediaFromDict.Width;
            media.Height = getMediaFromDict.Height;
        }
        
        await _dbContext.SaveChangesAsync();
        
        var updatedMedias = await _dbContext.ShopifyProductMedias
            .AsNoTracking()
            .Where(media => mediaListShopifyIds.Contains(media.ShopifyProductId))
            .Select(toDto => new ShopifyProductMediaDTO
            {
                MediaId = toDto.MediaId,
                ProductId = toDto.ProductId,
                ShopifyProductId = toDto.ShopifyProductId,
                Alt = toDto.Alt,
                MediaType = toDto.MediaType,
                ImageUrl = toDto.ImageUrl,
                Width = toDto.Width,
                Height = toDto.Height,
                CreatedAt = toDto.CreatedAt
            })
            .ToListAsync();

        return updatedMedias;
    }

    public Task<ShopifyProductDTO> CheckShopifyProductExistsByLinkAsync(string productLink, Guid storeId)
    {
        if (String.IsNullOrEmpty(productLink))
            throw new ArgumentNullException(nameof(productLink));
        
        if (Guid.Empty == storeId)
            throw new ArgumentNullException(nameof(storeId));
        
        throw new NotImplementedException();
    }
}