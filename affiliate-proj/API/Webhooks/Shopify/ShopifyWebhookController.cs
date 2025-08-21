using System.Text;
using affiliate_proj.Application.Interfaces.Shopify.Webhook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopifySharp;

namespace affiliate_proj.API.Webhooks.Shopify
{
    [Route("api/webhooks/[controller]")]
    [ApiController]
    public class ShopifyWebhookController : ControllerBase
    {
        private readonly IShopifyWebhookService _shopifyWebhookService;

        public ShopifyWebhookController(IShopifyWebhookService shopifyWebhookService)
        {
            _shopifyWebhookService = shopifyWebhookService;
        }
        
        [HttpPost("app/uninstalled")]
        public async Task<IActionResult> AppUninstalledAsync()
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
        
        [HttpPost("get-webhooks")]
        public async Task<IActionResult> GetWebhooksAsync([FromQuery] string shop, [FromQuery] string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
