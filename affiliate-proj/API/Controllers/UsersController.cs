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
        
        [HttpPost("update-email")]
        public async Task<ActionResult<UserDTO>> SetUserEmail([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.Email)) return NotFound();

            try
            {
                var user = await _accountService.SetEmailAsync(request.Email, request.UserId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("update-username")]
        public async Task<ActionResult<UserDTO>> SetUserName([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.Username)) return NotFound();

            try
            {
                var user = await _accountService.SetUserNameAsync(request.Username, request.UserId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("update-phone-number")]
        public async Task<ActionResult<UserDTO>> SetPhoneNumber([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.PhoneNumber)) return NotFound();

            try
            {
                var user = await _accountService.SetPhoneNumberAsync(request.PhoneNumber, request.UserId);
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
