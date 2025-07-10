using affiliate_proj.Application.Interfaces;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDTO = affiliate_proj.Core.DTOs.Account.UserDTO;

namespace affiliate_proj.API.Controllers
{
    // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid userId)
        {
            if (userId == Guid.Empty) return NotFound();
            try
            {
                var user = await _accountService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        /* Use only for testing. Do not use in production. */
        [HttpPost("by-email")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.Email)) return NotFound();

            try
            {
                var user = await _accountService.GetUserByEmailAsync(request.Email);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("set-user")]
        public async Task<ActionResult<UserDTO>> SetUser([FromBody] UserRequest request)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("update-email")]
        public async Task<ActionResult<UserDTO>> UpdateUserEmail([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.Email)) return NotFound();

            try
            {
                var user = await _accountService.UpdateEmailAsync(request.Email, request.UserId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("update-username")]
        public async Task<ActionResult<UserDTO>> UpdateUserName([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.Username)) return NotFound();

            try
            {
                var user = await _accountService.UpdateUserNameAsync(request.Username, request.UserId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("update-phone-number")]
        public async Task<ActionResult<UserDTO>> UpdatePhoneNumber([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.PhoneNumber)) return NotFound();

            try
            {
                var user = await _accountService.UpdatePhoneNumberAsync(request.PhoneNumber, request.UserId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpGet("/creator-profile/{userId}")]
        public async Task<ActionResult<CreatorDTO>> GetCreatorById(Guid userId)
        {
            if (userId == Guid.Empty) return NotFound();
            try
            {
                var creator = await _accountService.GetCreatorByUserIdAsync(userId);
                return Ok(creator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }
    }
}
