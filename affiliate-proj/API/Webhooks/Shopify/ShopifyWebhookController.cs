using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using affiliate_proj.Application.Interfaces.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

        public ShopifyWebhookController(IShopifyWebhookService shopifyWebhookService, 
            IShopifyRequestValidationUtility shopifyRequestValidationUtility, 
            IConfiguration configuration, IShopifyStoreHelper shopifyStoreHelper, 
            IStoreService storeService)
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
                    var store = await _shopifyStoreHelper.GetStoreDetailsByShopifyStoreIdAsync((long) shop.Id!);
                    // await _shopifyWebhookService.RemoveWebhookAsync(shop);
                    await _shopifyWebhookService.RemoveWebhooksAsync(store.StoreId);
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
                
                var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(body);
                Console.WriteLine($"Order Created:\nID: {order.Id} | Order Number: {order.OrderNumber}\n" +
                                  $"Referral: {order.Note}, {order.NoteAttributes.Count()}, " +
                                  $"{order.LandingSite}, {order.ReferringSite}\n" +
                                  $"Attributes: {order.Currency}, {order.FinancialStatus}, {order.FulfillmentStatus}, " +
                                  $"{order.CurrentSubtotalPrice}");
                
                var pretty = Newtonsoft.Json.JsonConvert.SerializeObject(
                    Newtonsoft.Json.JsonConvert.DeserializeObject(body), Newtonsoft.Json.Formatting.Indented);
                
                // Console.WriteLine(pretty);
                
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            
            throw new NotImplementedException();
        }

        [HttpPost("orders/cancelled")]
        public async Task<IActionResult> CancelOrderAsync()
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
                
                var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(body);
                
                var pretty = Newtonsoft.Json.JsonConvert.SerializeObject(
                    Newtonsoft.Json.JsonConvert.DeserializeObject(body), Newtonsoft.Json.Formatting.Indented);
                
                Console.WriteLine(pretty);
                
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            throw new NotImplementedException();
        }

        [HttpPost("orders/paid")]
        public async Task<IActionResult> PaidOrderAsync()
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
                
                var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(body);

                // foreach (var headers in Request.Headers)
                // {
                //     Console.WriteLine($"{headers.Key}: {headers.Value}");
                // }
                
                Console.WriteLine($"Store: {Request.Headers["X-Shopify-Shop-Domain"].ToString()}\n" +
                                  $"Order Paid:\nID: {order.Id} | Order Number: {order.OrderNumber}\n" +
                                  $"Referral: {order.Note}, {order.NoteAttributes.Count()}, " +
                                  $"{order.LandingSite}, {order.ReferringSite}\n" +
                                  $"Attributes: {order.Currency}, {order.FinancialStatus}, {order.FulfillmentStatus}, " +
                                  $"{order.CurrentSubtotalPrice}");
                
                var pretty = Newtonsoft.Json.JsonConvert.SerializeObject(
                    Newtonsoft.Json.JsonConvert.DeserializeObject(body), Newtonsoft.Json.Formatting.Indented);
                
                // Console.WriteLine(pretty);
                
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            
            throw new NotImplementedException();
        }

        [HttpPost("orders/fulfilled")]
        public async Task<IActionResult> FulfilledOrderAsync()
        {
            throw new NotImplementedException();
        }
        
        /* \\\\\\\\\\\\\\\ BELOW IS PREDOMINANTLY USED FOR DEV AND NOT INTENDED FOR PROD /////////////// */
        // TODO: Consider using [Authorize] for these endpoints below
        
        // Used for testing. DO NOT USE FOR PROD
        [HttpPost("set-single-webhook")]
        public async Task<IActionResult> SetSingleWebhookAsync([FromQuery] string shop, [FromQuery] string accessToken,
            [FromQuery] string newTopic)
        {
            try
            {
                await _shopifyWebhookService.RegisterWebhookAsync(shop, accessToken, newTopic);
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
        public async Task<IActionResult> UpdateAllWebhooksAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            try
            {
                await _shopifyWebhookService.UpdateAllWebhooksAsync(shop, accessToken);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete-single-webhook")]
        public async Task<IActionResult> DeleteSingleWebhookAsync([FromQuery] string shop, [FromQuery] string accessToken,
            [FromQuery] long webhookId)
        {
            try
            {
                await _shopifyWebhookService.RemoveWebhookAsync(shop, accessToken, webhookId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("delete-webhooks")]
        public async Task<IActionResult> DeleteAllWebhooksAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            try
            {
                await _shopifyWebhookService.RemoveAllWebhooksAsync(shop, accessToken);
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
