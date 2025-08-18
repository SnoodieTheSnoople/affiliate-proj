using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IShopifyAuthService _shopifyAuthService;
        private readonly IStoreService _storeService;

        public StoresController(IShopifyAuthService shopifyAuthService, IStoreService storeService)
        {
            _shopifyAuthService = shopifyAuthService;
            _storeService = storeService;
        }
        
        [HttpPost("set-store")]
        public async Task<ActionResult<Store>> SetStoreProfileAsync([FromBody] StoreDTO request)
        {
            if (request.UserId == Guid.Empty || String.IsNullOrEmpty(request.StoreName) 
                                             || request.ShopifyId == null 
                                             || String.IsNullOrEmpty(request.ShopifyToken)
                                             || String.IsNullOrEmpty(request.StoreUrl) 
                                             || String.IsNullOrEmpty(request.StoreName)
                                             || String.IsNullOrEmpty(request.ShopifyOwnerName)
                                             || String.IsNullOrEmpty(request.ShopifyOwnerEmail)
                                             || String.IsNullOrEmpty(request.ShopifyCountry)
                                             || String.IsNullOrEmpty(request.ShopifyGrantedScopes)) 
                return BadRequest();

            try
            {
                var addedStore = await _shopifyAuthService.SetShopifyStoreAsync(request);
                return addedStore;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("get-all-stores")]
        public async Task<ActionResult<List<Store>>> GetAllStoresAsync()
        {
            try
            {
                return await _storeService.GetAllStoresAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            throw new NotImplementedException();
        }
    }
}
