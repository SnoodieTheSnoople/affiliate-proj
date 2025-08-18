using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Core.DataTypes;
using affiliate_proj.Core.Entities;
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
    public async Task<Store> GetAllStoresAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<GraphResult<ListProductsResult>> GetProductsAsync(string shopDomain, string accessToken,
        int limit = 250)
    {
        // TODO: Change to factory pattern for GraphService instatiation.
        var graphService = new GraphService(shopDomain, accessToken);
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
                            variantsCount {
                                count
                            }
                        }
                    }
                }
                """,
            Variables = new Dictionary<string, object>
            {
                { "limit", 3 },
                { "query", "status:" + ProductStatus.ACTIVE }
            }
        };
        
        var graphResult = await graphService.PostAsync<ListProductsResult>(request);

        foreach (var node in graphResult.Data.Products.nodes)
        {
            if (node.id is not null)
                Console.WriteLine("Product ID is: " + node.id);
        }
        
        if ((bool)graphResult.Data.Products.pageInfo.hasNextPage)
            Console.WriteLine("Another Page of products available with cursor:" + 
                              graphResult.Data.Products.pageInfo.endCursor);
        
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