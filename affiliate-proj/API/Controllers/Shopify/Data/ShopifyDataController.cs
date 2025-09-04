using affiliate_proj.Application.Interfaces.Shopify.Data;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Shopify.Data;

[Route("api/data/[controller]")]
[ApiController]
public class ShopifyDataController : ControllerBase
{
    private readonly IShopifyProductService  _shopifyProductService;

    public ShopifyDataController(IShopifyProductService shopifyProductService)
    {
        _shopifyProductService = shopifyProductService;
    }
    
    [HttpGet("get-products")]
    public async Task<IActionResult> GetProductsAsync([FromQuery] string shop, [FromQuery] string accessToken)
    {
        try
        {
            return Ok(await _shopifyProductService.GetProductsAsync(shop, accessToken));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-products-count")]
    public async Task<IActionResult> GetProductsCountAsync([FromQuery] string shop, [FromQuery] string accessToken)
    {
        try
        {
            return Ok(await _shopifyProductService.GetProductsCountAsync(shop, accessToken));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("set-products")]
    public async Task<IActionResult> SetProductsAsync([FromQuery] string shop, [FromQuery] string accessToken)
    {
        throw new NotImplementedException();
    }
}