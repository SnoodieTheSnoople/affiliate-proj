using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp.Utilities;

namespace affiliate_proj.API.Controllers.Shopify
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IShopifyRequestValidationUtility _shopifyRequestValidationUtility;
        private readonly IShopifyDomainUtility _shopifyDomainUtility;

        public ShopifyController(IConfiguration configuration, 
            IShopifyRequestValidationUtility shopifyRequestValidationUtility, 
            IShopifyDomainUtility shopifyDomainUtility)
        {
            _configuration = configuration;
            _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
            _shopifyDomainUtility = shopifyDomainUtility;
        }
        
        [HttpGet("install")]
        public async Task<IActionResult> Install([FromQuery] string shop)
        {
            var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
            if (!isValidDomain) return BadRequest("Invalid shop domain");
            
            Console.WriteLine("Validated shop domain");
            return Ok();
            throw new NotImplementedException();
        }
    }
}
