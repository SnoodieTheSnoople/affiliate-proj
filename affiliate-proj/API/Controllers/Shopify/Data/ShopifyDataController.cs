using affiliate_proj.Application.Interfaces.Shopify.Data;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Shopify.Data;

[Route("api/[controller]")]
[ApiController]
public class ShopifyDataController : ControllerBase
{
    private readonly IShopifyProductService  _shopifyProductService;

    public ShopifyDataController(IShopifyProductService shopifyProductService)
    {
        _shopifyProductService = shopifyProductService;
    }
}