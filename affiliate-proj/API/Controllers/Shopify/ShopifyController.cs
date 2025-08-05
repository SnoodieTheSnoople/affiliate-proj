using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp.Enums;
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
        private readonly IShopifyOauthUtility _shopifyOauthUtility;

        public ShopifyController(IConfiguration configuration, 
            IShopifyRequestValidationUtility shopifyRequestValidationUtility, 
            IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility)
        {
            _configuration = configuration;
            _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
            _shopifyDomainUtility = shopifyDomainUtility;
            _shopifyOauthUtility = shopifyOauthUtility;
        }
        
        [HttpGet("install")]
        public async Task<IActionResult> Install([FromQuery] string shop)
        {
            var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
            if (!isValidDomain) return BadRequest("Invalid shop domain");
            
            Console.WriteLine("Validated shop domain");
            
            var configScopes = _configuration.GetValue<string>("Shopify:Scopes");
            var clientId = _configuration.GetValue<string>("Shopify:ClientId");
            var redirectUrl =  _configuration.GetValue<string>("Shopify:RedirectUrl");
            Console.WriteLine($"Scopes: {configScopes}\nClientId: {clientId}\nRedirectUrl: {redirectUrl}");
            
            var scopeAsList = configScopes.Split(",").ToList();

            var authUrl = _shopifyOauthUtility.BuildAuthorizationUrl(scopeAsList, shop, clientId, redirectUrl);
            
            return Redirect(authUrl.ToString());
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string shop, 
            [FromQuery] string state)
        {
            try
            {
                if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(shop))
                    return BadRequest("Missing parameters.");
                
                var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
                if(!isValidDomain) return BadRequest("Invalid shop domain");
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}
