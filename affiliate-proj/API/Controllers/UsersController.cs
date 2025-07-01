using affiliate_proj.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers
{
    // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public Task<ActionResult<string?>> GetUserById(System.Guid userId)
        {
            var user = _accountService.GetUserByIdAsync(userId);
            if (user == null) return Task.FromResult<ActionResult<string?>>(NotFound());
            
            return Task.FromResult<ActionResult<string?>>(Ok(user));
        }
    }
}
