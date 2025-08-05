using affiliate_proj.Application.Interfaces.Shopify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IShopifyAuthService _shopifyAuthService;

        public ShopifyController(IConfiguration configuration, 
            IShopifyRequestValidationUtility shopifyRequestValidationUtility, 
            IShopifyDomainUtility shopifyDomainUtility, IShopifyOauthUtility shopifyOauthUtility,
            IMemoryCache memoryCache, IShopifyAuthService shopifyAuthService)
        {
            _configuration = configuration;
            _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
            _shopifyDomainUtility = shopifyDomainUtility;
            _shopifyOauthUtility = shopifyOauthUtility;
            _memoryCache = memoryCache;
            _shopifyAuthService = shopifyAuthService;
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
            
            var redirectUrl = await _shopifyAuthService.GenerateInstallUrlAsync(shop);
            return Redirect(redirectUrl);
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
                
                return Ok(new {
                    shop, authorisation.AccessToken, authorisation.GrantedScopes
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
