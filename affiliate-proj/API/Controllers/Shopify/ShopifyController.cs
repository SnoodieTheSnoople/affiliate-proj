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
    }
}
