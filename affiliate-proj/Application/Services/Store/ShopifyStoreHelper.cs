using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Store;
using ShopifySharp;
using ShopifySharp.Utilities;

namespace affiliate_proj.Application.Services.Store;

public class ShopifyStoreHelper : IShopifyStoreHelper
{
    private readonly IShopifyAuthService _shopifyAuthService;
    private readonly IShopifyDomainUtility _shopifyDomainUtility;

    public ShopifyStoreHelper(IShopifyAuthService shopifyAuthService, IShopifyDomainUtility shopifyDomainUtility)
    {
        _shopifyAuthService = shopifyAuthService;
        _shopifyDomainUtility = shopifyDomainUtility;
    }
    public async Task<Shop?> GetShopifyStoreInfoAsync(string shop, string accessToken)
    {
        if (!await ValidateKeyProperties(shop, accessToken))
            return null;
        
        var shopService = new ShopService(shop, accessToken);
        var shopInfo = await shopService.GetAsync();
        
        // var propertyList = typeof(Shop).GetProperties().ToList();
        // foreach (var property in propertyList)
        // {
        //     var value = property.GetValue(shopInfo);
        //     Console.WriteLine($"{property} - {value}");
        // }
        
        return shopInfo;
    }
    
    private async Task<bool> ValidateKeyProperties(string shop, string accessToken)
    {
        var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
        if(!isValidDomain) 
            throw new Exception("Internal Error 001: Shopify Domain Not Valid");
        
        if (string.IsNullOrEmpty(accessToken))
            throw new Exception("Internal Error 007: Invalid access token");
        
        return true;
    }
}