using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers.Shopify
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopifyController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private const string StateSessionKey = "Shopify_OAuthState";
        
        public ShopifyController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _client = clientFactory.CreateClient();
        }
        
        [HttpGet("install")]
        public IActionResult Install([FromQuery] string shop)
        {
            if (!IsValidShopDomain(shop)) return BadRequest("Invalid domain.");
            
            var scopes = _configuration.GetValue<string>("Shopify:Scopes");
            scopes = Uri.EscapeDataString(scopes!);
            
            var clientId = _configuration.GetValue<string>("Shopify:ClientId");
            var redirectUrl = Uri.EscapeDataString(_configuration.GetValue<string>("Shopify:RedirectUrl")!);
            
            /*TODO: Add better nonce/state generation for security*/
            var state = Guid.NewGuid().ToString();
            Console.WriteLine(state);
            HttpContext.Session.SetString(StateSessionKey, state);

            var authUrl =
                $"https://{shop}/admin/oauth/authorize?client_id={clientId}" +
                $"&scope={scopes}" +
                $"&redirect_uri={redirectUrl}" +
                $"&state={state}" +
                $"&grant_options[]=per-user";
            
            return Redirect(authUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string shop, [FromQuery(Name = "code")] string authCode,
            [FromQuery] string hmac, [FromQuery] string? scope, [FromQuery] string state)
        {
            try
            {
                Console.WriteLine(Request.QueryString.Value);
                var savedState = HttpContext.Session.GetString("Shopify_OAuthState");
                Console.WriteLine(savedState);
                Console.WriteLine(state);
                if (!String.Equals(savedState, state))
                    return BadRequest("Invalid state parameter.");
                
                if (!IsValidHmac(Request.Query, _configuration.GetValue<string>("Shopify:ApiSecret")))
                    return Unauthorized("HMAC validation failed.");

                var accessToken = await GetAccessToken(shop, authCode);

                if (String.IsNullOrEmpty(accessToken)) return Unauthorized("Invalid access token.");

                return Ok(new { shop, accessToken });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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

        private async Task<string?> GetAccessToken(string shop, string authCode)
        {
            try
            {
                var tokenUrl = $"https://{shop}/admin/oauth/access_token";
                var payload = new
                {
                    client_id = _configuration.GetValue<string>("Shopify:ClientId"),
                    client_secret = _configuration.GetValue<string>("Shopify:ApiSecret"),
                    code = authCode
                };
                // Console.WriteLine("URL: " + tokenUrl);
                // Console.WriteLine("Payload: " + payload + "\n");
                
                var response = await _client.PostAsJsonAsync(tokenUrl, payload);
                var content = await response.Content.ReadFromJsonAsync<JsonElement>();
                
                return content.TryGetProperty("access_token", out var accessToken) ? accessToken.GetString() : null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private async Task<List<string>> GetScopes(string shop, string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
