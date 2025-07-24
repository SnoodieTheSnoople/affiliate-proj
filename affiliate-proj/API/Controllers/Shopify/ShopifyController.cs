using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
            if (!IsValidHmac(Request.Query, _configuration.GetValue<string>("Shopify:ApiSecret"))
                return Unauthorized("HMAC validation failed.");
            
            
        }

        private bool IsValidShopDomain(string domain)
        {
            return Regex.IsMatch(domain, @"^[a-zA-Z0-9-_][a-zA-Z0-9-_]*\.myshopify\.com$");
        }

        private bool IsValidHmac(IQueryCollection query, string secret)
        {
            var getParameters = query.Where(param => param.Key != "hmac" && param.Key != "signature")
                .OrderBy(param => param.Key, StringComparer.Ordinal)
                .Select(param => $"{param.Key}={param.Value}");
            
            var parameterAsString = string.Join("&", getParameters);
            
            var hmacHasher = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var computedHash = hmacHasher.ComputeHash(Encoding.UTF8.GetBytes(parameterAsString));
            
            var computedHmac = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
            var receivedHmac = query["hmac"].ToString();
            
            return String.Equals(computedHmac, receivedHmac, StringComparison.Ordinal);
        }

        private async Task<string?> GetAccessToken(string shop, string code)
        {
            var tokenUrl = $"https://{shop}/admin/oauth/access_token";
            var payload = new
            {
                client_id = _configuration.GetValue<string>("Shopify:ClientId"),
                client_secret = _configuration.GetValue<string>("Shopify:ApiSecret"),
                code
            };
            
            var response = await _client.PostAsJsonAsync(tokenUrl, payload);
            var content = await response.Content.ReadFromJsonAsync<JsonElement>();
            
            return content.TryGetProperty("access_token", out var accessToken) ? 
                accessToken.GetString() : null;
        }
    }
}
