using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Store;
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
        private readonly IShopifyStoreHelper _shopifyStoreHelper;
        private readonly IAccountHelper _accountHelper;

        public ShopifyController(IShopifyAuthService shopifyAuthService, IShopifyDataService shopifyDataService,
            IShopifyStoreHelper shopifyStoreHelper, IAccountHelper accountHelper)
        {
            _shopifyAuthService = shopifyAuthService;
            _shopifyDataService = shopifyDataService;
            _shopifyStoreHelper = shopifyStoreHelper;
            _accountHelper = accountHelper;
        }
        
        [HttpGet("install")]
        public async Task<IActionResult> Install([FromQuery] string shop)
        {
            try
            {
                var userId = _accountHelper.GetUserIdFromAccessToken();
                if (String.IsNullOrEmpty(userId)) 
                    return BadRequest();
                
                // var redirectUrl = await _shopifyAuthService.GenerateInstallUrlAsync(shop);
                var redirectUrl = await _shopifyAuthService.GenerateInstallUrlWithUserIdAsync(shop, 
                    Guid.Parse(userId));
                // return Redirect(redirectUrl);
                // TODO: Add check before adding to db
                return Ok(redirectUrl);
            }
            catch (Exception e)
            {
                // TODO: Create exception handling system according to custom codes.
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string shop, 
            [FromQuery] string state)
        {
            try
            {
                var authorisation = await _shopifyAuthService.HandleCallbackAsync(code, shop, state,
                    Request.QueryString.ToString());

                var shopInfo = await _shopifyStoreHelper.GetShopifyStoreInfoAsync(shop, authorisation.AccessToken);
                if (shopInfo == null)
                    return BadRequest("Invalid Shopify Store ID");
                
                // var addedStore = await _shopifyAuthService.SetShopifyStoreAsync(shopInfo, authorisation);
                
                return Ok(new {
                    shop, authorisation.AccessToken, authorisation.GrantedScopes,shopInfo.Name, shopInfo.Email,
                    shopInfo.Country, shopInfo.Domain, shopInfo.Id, shopInfo.Phone, shopInfo.Description,
                    JoinedGrantedScopes = String.Join(",", authorisation.GrantedScopes)
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
