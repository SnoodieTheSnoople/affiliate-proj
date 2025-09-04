using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Store;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Shopify;

[Route("api/[controller]")]
[ApiController]
public class ShopifyController : ControllerBase
{
    private readonly IShopifyAuthService _shopifyAuthService;
    private readonly IShopifyStoreHelper _shopifyStoreHelper;
    private readonly IAccountHelper _accountHelper;

    public ShopifyController(IShopifyAuthService shopifyAuthService,
        IShopifyStoreHelper shopifyStoreHelper, IAccountHelper accountHelper)
    {
        _shopifyAuthService = shopifyAuthService;
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
                return Unauthorized();

            if (!await _accountHelper.CheckUserIsStoreOwnerAsync(Guid.Parse(userId)))
                return Unauthorized();
                
            var redirectUrl = await _shopifyAuthService.GenerateInstallUrlWithUserIdAsync(shop, 
                Guid.Parse(userId));
                
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
}