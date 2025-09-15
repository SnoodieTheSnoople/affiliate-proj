using affiliate_proj.Application.Interfaces.Shopify.Data;
using affiliate_proj.Application.Interfaces.Shopify.Data.Factories;
using affiliate_proj.Core.DataTypes.GraphQL;
using ShopifySharp;
using ShopifySharp.GraphQL;
using ShopifySharp.Services.Graph;

namespace affiliate_proj.Application.Services.Shopify.Data;

public class ShopifyProductService : IShopifyProductService
{
    private readonly IGraphServiceFactory _graphServiceFactory;

    public ShopifyProductService(IGraphServiceFactory graphServiceFactory)
    {
        _graphServiceFactory = graphServiceFactory;
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
        
        var productsCount = countsResult.Data.ProductsCount.count;

        if (productsCount == 0)
            return;
        
        if (productsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(productsCount));
        
        var restoreRate = countsResult.Extensions.Cost.ThrottleStatus.RestoreRate;
        var maximumAvailable = countsResult.Extensions.Cost.ThrottleStatus.MaximumAvailable;
        var currentAmt = maximumAvailable - countsResult.Extensions.Cost.ActualQueryCost;
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
        
        Console.WriteLine($"Restore Rate: {restoreRate} |  Maximum Available: {maximumAvailable} |  Current Amt: {currentAmt}");
        
        var productsResult = await GetProductsAsync(shopDomain, accessToken);
        
        var allProducts = productsResult.Data.Products.Nodes.ToList();

        foreach (var product in allProducts)
        {
            Console.WriteLine($"Product ID: {product.Id}, Product Title: {product.Title}");
            foreach (var media in product.Media.Nodes)
            {
                Console.WriteLine($"Media img ID: {media.Id}, Media: {media.MediaContentType}, Img Url: {media.Preview.Image.Url}");
            }
        }
    }

    public Task<double> GetTimeDelayForFullRecovery(double maxAvailable, double currentAmtAvailabe, double restoreRate)
    {
        throw new NotImplementedException();
    }
}