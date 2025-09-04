using affiliate_proj.Application.Interfaces.Shopify.Data;
using affiliate_proj.Core.DataTypes.GraphQL;
using ShopifySharp;
using ShopifySharp.GraphQL;
using ShopifySharp.Services.Graph;

namespace affiliate_proj.Application.Services.Shopify.Data;

public class ShopifyProductService : IShopifyProductService
{
    public async Task<int> GetProductsCount(string shopDomain, string accessToken)
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
        return graphResult.Data.ProductsCount.count;
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
}