using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Shopify
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public ShopifyController(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        [HttpGet("install")]
        public IActionResult Install([FromQuery] string shop)
        {
            if (!IsValidShopDomain(shop)) return BadRequest("Invalid domain.");
            
            var scopes = _configuration.GetSection("Shopify:Scopes").Get<string[]>();
            var clientId = _configuration.GetValue<string>("Shopify:ClientId");
            var redirectUrl = Uri.EscapeDataString(_configuration.GetValue<string>("Shopify:RedirectUrl")!);

            var authUrl =
                $"https://{shop}/admin/oauth/authorize?client_id={clientId}&scope={scopes}&redirect_uri={redirectUrl}";
            
            return Redirect(authUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string shop, [FromQuery] string code,
            [FromQuery] string hmac)
        {
            throw new NotImplementedException();
        }

        private bool IsValidShopDomain(string domain)
        {
            return Regex.IsMatch(domain, @"^[a-zA-Z0-9-_][a-zA-Z0-9-_]*\.myshopify\.com$");
        }
    }
}
