using affiliate_proj.Application.Interfaces.Shopify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp.Enums;

namespace affiliate_proj.API.Controllers.Shopify
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopifyController : ControllerBase
    {
        private readonly IShopifyAuthService _shopifyAuthService;
        private readonly IShopifyDataService _shopifyDataService;

        public ShopifyController(IShopifyAuthService shopifyAuthService, IShopifyDataService shopifyDataService)
        {
            _shopifyAuthService = shopifyAuthService;
            _shopifyDataService = shopifyDataService;
        }
        
        [HttpGet("install")]
        public async Task<IActionResult> Install([FromQuery] string shop)
        {
            // var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
            // if (!isValidDomain) return BadRequest("Invalid shop domain");
            //
            // Console.WriteLine("Validated shop domain");
            //
            // var configScopes = _configuration.GetValue<string>("Shopify:Scopes");
            // var clientId = _configuration.GetValue<string>("Shopify:ClientId");
            // var redirectUrl =  _configuration.GetValue<string>("Shopify:RedirectUrl");
            // Console.WriteLine($"Scopes: {configScopes}\nClientId: {clientId}\nRedirectUrl: {redirectUrl}");
            //
            // var scopeAsList = configScopes.Split(",").ToList();
            //
            // var state = Guid.NewGuid().ToString();
            //
            // _memoryCache.Set("ShopifyOAuthState", state);
            //
            // var authUrl = _shopifyOauthUtility.BuildAuthorizationUrl(scopeAsList, shop, 
            //     clientId, redirectUrl, state);
            //
            // return Redirect(authUrl.ToString());
            try
            {
                var redirectUrl = await _shopifyAuthService.GenerateInstallUrlAsync(shop);
                return Redirect(redirectUrl);
            }
            catch (Exception e)
            {
                // TODO: Create exception handling system according to custom codes.
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string shop, 
            [FromQuery] string state)
        {
            try
            {
                // if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(shop))
                //     return BadRequest("Missing parameters.");
                //
                // var isValidDomain = await _shopifyDomainUtility.IsValidShopDomainAsync(shop);
                // if(!isValidDomain) return BadRequest("Invalid shop domain");
                //
                // var savedState = _memoryCache.Get("ShopifyOAuthState").ToString();
                // if (String.IsNullOrEmpty(savedState)) return StatusCode(500, "No saved state");
                //
                // Console.WriteLine($"Saved State: {savedState}");
                //
                // if (!String.Equals(savedState, state)) return BadRequest("Invalid state");
                //
                // var clientId = _configuration.GetValue<string>("Shopify:ClientId");
                // var apiSecret = _configuration.GetValue<string>("Shopify:ApiSecret");
                //
                // var isValidRequest = _shopifyRequestValidationUtility.IsAuthenticRequest(
                //     Request.QueryString.ToString(), apiSecret);
                //
                // if (!isValidRequest)
                // {
                //     Console.WriteLine("Invalid request");
                //     return BadRequest("Invalid request");
                // }
                //
                // var authorisation = await _shopifyOauthUtility.AuthorizeAsync(code, shop, clientId, apiSecret);
                //
                // if (string.IsNullOrEmpty(authorisation.AccessToken)) return BadRequest("Invalid access_token");
                // Console.WriteLine($"Obtained access_token: {authorisation.AccessToken}");
                // Console.WriteLine($"Obtained access scope: ");
                // foreach (string obtainedScope in authorisation.GrantedScopes)
                // {
                //     Console.WriteLine(obtainedScope);
                // }

                var authorisation = await _shopifyAuthService.HandleCallbackAsync(code, shop, state,
                    Request.QueryString.ToString());

                var shopInfo = await _shopifyAuthService.GetShopifyStoreIdAsync(shop, authorisation.AccessToken);
                if (shopInfo == null)
                    return BadRequest("Invalid Shopify Store ID");
                
                var addedStore = await _shopifyAuthService.SetShopifyStoreAsync(shopInfo, authorisation);
                
                return Ok(new {
                    shop, authorisation.AccessToken, authorisation.GrantedScopes,shopInfo.Name, shopInfo.Email,
                    shopInfo.Country, shopInfo.Domain, shopInfo.Id, shopInfo.Phone, shopInfo.Description,
                    addedStore
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProductsAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            try
            {
                return Ok(await _shopifyDataService.GetProductsAsync(shop, accessToken));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
