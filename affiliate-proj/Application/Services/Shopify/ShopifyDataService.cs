using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Shopify.Data.Factories;
using affiliate_proj.Core.DataTypes.GraphQL;
using ShopifySharp;
using ShopifySharp.GraphQL;
using ShopifySharp.Services.Graph;
using Fulfillment = ShopifySharp.Fulfillment;
using Order = ShopifySharp.Order;
using Refund = ShopifySharp.Refund;
using Shop = ShopifySharp.Shop;

namespace affiliate_proj.Application.Services.Shopify;

public class ShopifyDataService : IShopifyDataService
{
    private readonly IGraphServiceFactory _graphServiceFactory;

    public ShopifyDataService(IGraphServiceFactory graphServiceFactory)
    {
        _graphServiceFactory = graphServiceFactory;
    }


    public async Task<int> GetProductsCount(string shopDomain, string accessToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GraphResult<CustomListProductsResult>> GetProductsAsync(string shopDomain, string accessToken,
        int limit = 50)
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

    public Task<IEnumerable<Order>> GetOrdersAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Fulfillment>> GetFulfillmentsAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Refund>> GetRefundsAsync(string shopDomain, string accessToken, int limit = 250)
    {
        throw new NotImplementedException();
    }

    public Task<Shop> GetShopAsync(string shopDomain, string accessToken)
    {
        throw new NotImplementedException();
    }
}