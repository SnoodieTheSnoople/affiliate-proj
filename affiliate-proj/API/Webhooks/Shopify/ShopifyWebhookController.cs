using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Application.Interfaces.Store;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp;
using ShopifySharp.Utilities;

namespace affiliate_proj.API.Webhooks.Shopify
{
    [Route("api/webhooks/[controller]")]
    [ApiController]
    public class ShopifyWebhookController : ControllerBase
    {
        private readonly IShopifyWebhookService _shopifyWebhookService;
        private readonly IShopifyRequestValidationUtility  _shopifyRequestValidationUtility;
        private readonly IConfiguration _configuration;
        private readonly IShopifyStoreHelper _shopifyStoreHelper;
        private readonly IStoreService _storeService;

        public ShopifyWebhookController(IShopifyWebhookService shopifyWebhookService, IShopifyRequestValidationUtility shopifyRequestValidationUtility, IConfiguration configuration, IShopifyStoreHelper shopifyStoreHelper, IStoreService storeService)
        {
            _shopifyWebhookService = shopifyWebhookService;
            _shopifyRequestValidationUtility = shopifyRequestValidationUtility;
            _configuration = configuration;
            _shopifyStoreHelper = shopifyStoreHelper;
            _storeService = storeService;
        }
        
        [HttpPost("app/uninstalled")]
        public async Task<IActionResult> AppUninstalledAsync()
        {
            try
            {
                Request.EnableBuffering();
            
                using var reader = new StreamReader(Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
            
                Request.Body.Position = 0;
            
                var isValid = await _shopifyRequestValidationUtility.IsAuthenticWebhookAsync(Request.Headers, Request.Body,
                    _configuration.GetValue<string>("Shopify:ApiSecret"));
            
                if (!isValid)
                    return BadRequest();
            
                var shop = Newtonsoft.Json.JsonConvert.DeserializeObject<Shop>(body);

                if (shop != null)
                {
                    await _shopifyWebhookService.RemoveWebhookAsync(shop);
                    var store = await _shopifyStoreHelper.GetStoreDetailsByShopifyStoreIdAsync((long) shop.Id!);
                    await _storeService.DeleteStoreAsync(store.StoreId);
                    // TODO: Simply this condition and refactor RemoveWebhookAsync or overload method with StoreId
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("orders/create")]
        public async Task<IActionResult> CreateOrderAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("orders/cancelled")]
        public async Task<IActionResult> CancelOrderAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("orders/paid")]
        public async Task<IActionResult> PaidOrderAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("orders/fulfilled")]
        public async Task<IActionResult> FulfilledOrderAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("set-webhook")]
        public async Task<IActionResult> SetWebhookAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            try
            {
                await _shopifyWebhookService.RegisterWebhookAsync(shop, accessToken);
                var webhooks = await _shopifyWebhookService.GetAllWebhooksAsync(shop, accessToken);
                return Ok(webhooks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("set-webhooks")]
        public async Task<IActionResult> SetWebhooksAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            try
            {
                await _shopifyWebhookService.RegisterWebhooksAsync(shop, accessToken);
                var webhooks = await _shopifyWebhookService.GetAllWebhooksAsync(shop, accessToken);
                return Ok(webhooks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("get-webhooks")]
        public async Task<IActionResult> GetWebhooksAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            try
            {
                return Ok(await _shopifyWebhookService.GetAllWebhooksAsync(shop, accessToken));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("update-webhooks")]
        public async Task<IActionResult> UpdateAllWebhooksAsync([FromQuery] string shop, [FromQuery] string accessToken,
            [FromQuery] long webhookId)
        {
            try
            {
                await _shopifyWebhookService.UpdateAllWebhookAsync(shop, accessToken, webhookId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            throw new NotImplementedException();
        }

        [HttpDelete("delete-webhook")]
        public async Task<IActionResult> DeleteWebhookAsync([FromQuery] string shop, [FromQuery] string accessToken, [FromQuery] long webhookId)
        {
            try
            {
                // TODO: Implement delete webhook functionality for db
                await _shopifyWebhookService.RemoveWebhookAsync(shop, accessToken, webhookId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
