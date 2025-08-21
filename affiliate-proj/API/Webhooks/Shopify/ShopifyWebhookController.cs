using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Webhooks.Shopify
{
    [Route("api/webhooks/[controller]")]
    [ApiController]
    public class ShopifyWebhookController : ControllerBase
    {
        [HttpPost("app/uninstalled")]
        public async Task<IActionResult> AppUninstalledAsync()
        {
            throw new NotImplementedException();
        }
    }
}
