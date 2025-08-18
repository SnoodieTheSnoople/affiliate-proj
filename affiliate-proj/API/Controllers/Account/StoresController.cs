using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Shopify;
using affiliate_proj.Application.Interfaces.Store;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoresController : ControllerBase
    {
        private readonly IShopifyAuthService _shopifyAuthService;
        private readonly IStoreService _storeService;
        private readonly IAccountHelper _accountHelper;

        public StoresController(IShopifyAuthService shopifyAuthService, IStoreService storeService, IAccountHelper accountHelper)
        {
            _shopifyAuthService = shopifyAuthService;
            _storeService = storeService;
            _accountHelper = accountHelper;
        }
        
        [HttpPost("set-store")]
        public async Task<ActionResult<CreateStoreDTO>> SetStoreProfileAsync([FromBody] CreateStoreDTO request)
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
        public async Task<ActionResult<List<StoreDTO>>> GetAllStoresAsync()
        {
            if (!ValidateUser())
                return Unauthorized("User does not exist.");
            try
            {
                return await _storeService.GetAllStoresAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("get-store/{storeId}")]
        public async Task<ActionResult<Store>> GetStoreByIdAsync(Guid storeId)
        {
            if (!ValidateUser())
                return Unauthorized("User does not exist.");
            try
            {
                return await _storeService.GetStoreByIdAsync(storeId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                BadRequest(e.Message);
            }
            throw new NotImplementedException();
        }

        private bool ValidateUser()
        {
            var userId = Guid.Parse(_accountHelper.GetUserIdFromAccessToken());
            return _accountHelper.CheckUserExists(userId);
        }
    }
}
