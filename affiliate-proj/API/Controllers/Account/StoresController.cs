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
        [HttpPost("/set-store")]
        public async Task<ActionResult<Store>> SetStoreProfile([FromBody] StoreDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
