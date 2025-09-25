using affiliate_proj.Application.Interfaces.Shopify.Data;
using affiliate_proj.Application.Interfaces.Shopify.Data.Factories;
using affiliate_proj.Application.Interfaces.Shopify.Data.Product;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DataTypes.GraphQL;
using affiliate_proj.Core.DTOs.Shopify.Products;
using affiliate_proj.Core.DTOs.Shopify.Products.Media;
using ShopifySharp;
using ShopifySharp.GraphQL;
using ShopifySharp.Services.Graph;

namespace affiliate_proj.Application.Services.Shopify.Data;

public class ShopifyProductService : IShopifyProductService
{
    private readonly IGraphServiceFactory _graphServiceFactory;
    private readonly IShopifyStoreHelper _shopifyStoreHelper;
    private readonly IShopifyProductRepository _shopifyProductRepository;

    public ShopifyProductService(IGraphServiceFactory graphServiceFactory, IShopifyStoreHelper shopifyStoreHelper, IShopifyProductRepository shopifyProductRepository)
    {
        _graphServiceFactory = graphServiceFactory;
        _shopifyStoreHelper = shopifyStoreHelper;
        _shopifyProductRepository = shopifyProductRepository;
    }

    public async Task<GraphResult<ProductsCountResult>> GetProductsCountAsync(string shopDomain, string accessToken)
    {
        var graphService = _graphServiceFactory.CreateGraphService(shopDomain, accessToken);
        var request = new GraphRequest
        {
            Query =
                """
                query getProductCount($query: String!) {
                    productsCount(query: $query) {
                        count
                    }
                }
                """,
            Variables = new Dictionary<string, object>
            {
                { "query", "status:" + ProductStatus.ACTIVE}
            }
        };
        
        var graphResult = await graphService.PostAsync<ProductsCountResult>(request);
        return graphResult;
    }

    public async Task<GraphResult<CustomListProductsResult>> GetProductsAsync(string shopDomain, string accessToken,
        int limit = 250)
    {
        var graphService = _graphServiceFactory.CreateGraphService(shopDomain, accessToken);
        var request = new GraphRequest
        {
            Query =
                """
                query listProducts($limit: Int!, $query: String!) {
                    products(first: $limit, query: $query) {
                        pageInfo {
                            startCursor
                            endCursor
                            hasNextPage
                            hasPreviousPage
                        }
                        nodes {
                            id
                            title
                            handle
                            hasOnlyDefaultVariant
                            onlineStoreUrl
                            media(first:1) {
                                nodes {
                                        id
                                        alt
                                        mediaContentType
                                        preview {
                                            image {
                                                altText
                                                height
                                                id
                                                url
                                                width
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                """,
            Variables = new Dictionary<string, object>
            {
                { "limit", limit },
                { "query", "status:" + ProductStatus.ACTIVE }
            }
        };
        
        var graphResult = await graphService.PostAsync<CustomListProductsResult>(request);
        
        return graphResult;
    }

    public async Task SetProductsAsync(string shopDomain, string accessToken)
    {
        var countsResult = await GetProductsCountAsync(shopDomain,  accessToken);
        var storeDetails = await _shopifyStoreHelper.GetStoreByDomainAsync(shopDomain);
        
        var productsCount = countsResult.Data.ProductsCount.count;

        if (productsCount == 0)
            return;
        
        if (productsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(productsCount));

        var productsResult = await GetProductsAsync(shopDomain, accessToken);
        
        var restoreRate = countsResult.Extensions.Cost.ThrottleStatus.RestoreRate;
        var maximumAvailable = countsResult.Extensions.Cost.ThrottleStatus.MaximumAvailable;
        var currentAmt = maximumAvailable - countsResult.Extensions.Cost.ActualQueryCost;
        var actualCost = countsResult.Extensions.Cost.ActualQueryCost;

        Console.WriteLine($"Restore Rate: {restoreRate} |  Maximum Available: {maximumAvailable} |  Current Amt: {currentAmt}");

        if (productsCount > 250)
        {
            // var restoreRate = countsResult.Extensions.Cost.ThrottleStatus.RestoreRate;
            // var maximumAvailable = countsResult.Extensions.Cost.ThrottleStatus.MaximumAvailable;
            // var currentAmt = maximumAvailable - countsResult.Extensions.Cost.ActualQueryCost;
            Console.WriteLine($"Restore Rate: {restoreRate} |  Maximum Available: {maximumAvailable} |  Current Amt: {currentAmt}");
            //TODO: Delay next API call, increment by cursor.
        }

        //TODO: Repository call, increment 


        restoreRate = productsResult.Extensions.Cost.ThrottleStatus.RestoreRate;
        maximumAvailable = productsResult.Extensions.Cost.ThrottleStatus.MaximumAvailable;
        currentAmt = maximumAvailable - productsResult.Extensions.Cost.ActualQueryCost;
        actualCost = productsResult.Extensions.Cost.ActualQueryCost;
        
        Console.WriteLine($"Restore Rate: {restoreRate} | Maximum Available: {maximumAvailable} | " +
                          $"Current Amt: {currentAmt} | Actual Cost: {actualCost}");
        
        Console.WriteLine(GetTimeDelayForFullRecovery(maximumAvailable, currentAmt, restoreRate));
        Console.WriteLine( currentAmt < actualCost ? GetTimeDelayForNextQuery(currentAmt, restoreRate, actualCost) : 0);
        
        var allProducts = productsResult.Data.Products.Nodes.ToList();
        var productsList = new List<ShopifyProductDTO>();
        var mediaList = new List<CreateShopifyProductMediaDTO>();

        foreach (var product in allProducts)
        {
            Console.WriteLine($"Product ID: {product.Id}, Product Title: {product.Title}");
            var generatedProductId = Guid.NewGuid();
            productsList.Add(new ShopifyProductDTO
            {
                ProductId = generatedProductId,
                StoreId = storeDetails.StoreId,
                ShopifyProductId = product.Id,
                Title = product.Title,
                Handle = product.Handle,
                HasOnlyDefaultVariant = product.HasOnlyDefaultVariant,
                OnlineStoreUrl = product.OnlineStoreUrl,
            });
            foreach (var media in product.Media.Nodes)
            {
                Console.WriteLine($"Media img ID: {media.Id}, Media: {media.MediaContentType}, Img Url: {media.Preview.Image.Url}");
                mediaList.Add(new CreateShopifyProductMediaDTO
                {
                    ProductId = generatedProductId,
                    Alt = media.Alt,
                    MediaType = media.MediaContentType,
                    ImageUrl = media.Preview.Image.Url,
                    Width = (int)media.Preview.Image.Width,
                    Height = (int)media.Preview.Image.Height,
                });
            }
        }
        
        //ITERATE THROUGH PRODUCTSLIST AND MEDIALIST AND ADD TO DB
        await SetProductsInDbAsync(productsList);
        await SetProductMediaInDbAsync(mediaList);
        // TODO: Implement SetProductsInDbAsync & SetProductMediaInDbAsync
    }

    private async Task<List<ShopifyProductDTO>> SetProductsInDbAsync(List<ShopifyProductDTO> productsDtos)
    {
        throw new NotImplementedException();
    }

    private async Task<List<ShopifyProductMediaDTO>> SetProductMediaInDbAsync(
        List<CreateShopifyProductMediaDTO> productMediaDtos)
    {
        throw new NotImplementedException();
    }

    private double GetTimeDelayForFullRecovery(double maxAvailable, double currentAmtAvailabe, double restoreRate)
    {
        return (maxAvailable - currentAmtAvailabe) / restoreRate;
    }

    private double GetTimeDelayForNextQuery(double currentAmtAvailabe, double restoreRate,
        double actualCost)
    {
        var bufferMultiplier = 1.5;
        var overhead = 50;
        var bufferedCost = actualCost * bufferMultiplier + overhead;
        
        var delta = bufferedCost - currentAmtAvailabe;

        var timeDelayResult = delta / restoreRate;
        return timeDelayResult;
    }
}