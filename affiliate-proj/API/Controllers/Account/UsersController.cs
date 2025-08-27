using affiliate_proj.Application.Interfaces;
using affiliate_proj.Application.Interfaces.Account;
using affiliate_proj.Application.Interfaces.Account.Creator;
using affiliate_proj.Application.Interfaces.Account.User;
using affiliate_proj.Core.DTOs.Account;
using affiliate_proj.Core.Entities;
using affiliate_proj.Core.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDTO = affiliate_proj.Core.DTOs.Account.UserDTO;

namespace affiliate_proj.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICreatorService _creatorService;
        private readonly IUserService _userService;

        public UsersController(IAccountService accountService, ICreatorService creatorService, IUserService userService)
        {
            _accountService = accountService;
            _creatorService = creatorService;
            _userService = userService;
        }

        [HttpPost("set-user")]
        public async Task<ActionResult<UserDTO>> SetUser([FromBody] UserRequest request)
        {
            try
            {
                var newUserObjectMap = new UserDTO
                {
                    UserId = request.UserId,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Username = request.Username,
                };

                var returnedUserFromRequest = await _userService.SetUserAsync(newUserObjectMap, request.UserId);
                return returnedUserFromRequest;
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid userId)
        {
            if (userId == Guid.Empty) return NotFound();
            try
            {
                var user = await _userService.GetUserByUserIdAsync(userId);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpDelete("delete-user")]
        public async Task<ActionResult<UserDTO>> DeleteUser([FromBody] UserRequest request)
        {
            try
            {
                var user = await _accountService.DeleteUser(request.UserId);
                return user;
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }
        
        [HttpPost("update-email")]
        public async Task<ActionResult<UserDTO>> UpdateUserEmail([FromBody] UserRequest request)
        {
            if (String.IsNullOrEmpty(request.Email)) return NotFound();

            try
            {
                var user = await _userService.UpdateEmailAsync(request.Email, request.UserId);
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
                var user = await _userService.UpdateUserNameAsync(request.Username, request.UserId);
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
                var user = await _userService.UpdatePhoneNumberAsync(request.PhoneNumber, request.UserId);
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
                var creator = await _creatorService.GetCreatorByUserIdAsync(userId);
                return Ok(creator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("/update-firstname")]
        public async Task<ActionResult<CreatorDTO>> UpdateFirstName([FromBody] CreatorRequest request)
        {
            if (request.UserId == Guid.Empty) return NotFound();
            if (String.IsNullOrEmpty(request.Firstname)) return NotFound();

            try
            {
                var creator = await _creatorService.UpdateFirstNameAsync(request.Firstname, request.UserId);
                return Ok(creator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("/update-lastname")]
        public async Task<ActionResult<CreatorDTO>> UpdateLastName([FromBody] CreatorRequest request)
        {
            if (request.UserId == Guid.Empty) return NotFound();
            if (String.IsNullOrEmpty(request.Surname)) return NotFound();

            try
            {
                var creator = await _creatorService.UpdateSurnameAsync(request.Surname, request.UserId);
                return Ok(creator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("/update-dob")]
        public async Task<ActionResult<CreatorDTO>> UpdateDob([FromBody] CreatorRequest request)
        {
            if (request.UserId == Guid.Empty) return NotFound();
            if (request.Dob == null) return NotFound();

            try
            {
                DateOnly castedDob = request.Dob.Value;
                var creator = await _creatorService.UpdateDateOfBirthAsync(castedDob, request.UserId);
                return Ok(creator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/set-creator")]
        public async Task<ActionResult<CreatorDTO>> SetCreator([FromBody] CreatorRequest request)
        {
            if (request.UserId == Guid.Empty) return NotFound();
            if (String.IsNullOrEmpty(request.Firstname)) 
                return NotFound();
            
            if (String.IsNullOrEmpty(request.Surname))
                return NotFound();
            
            if (request.Dob == null) return NotFound();

            if (String.IsNullOrEmpty(request.StripeId))
                return NotFound();

            try
            {
                DateOnly castedDob = request.Dob.Value;
                var setCreator = new CreatorDTO
                {
                    Firstname = request.Firstname,
                    Surname = request.Surname,
                    Dob = castedDob,
                    StripeId = request.StripeId,
                    UserId = request.UserId
                };

                var creator = await _creatorService.SetCreatorAsync(setCreator, request.UserId);
                return Ok(creator);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/delete-profile")]
        public async Task<ActionResult<ProfileDTO>> DeleteProfile([FromBody] UserRequest request)
        {
            if (request.UserId == Guid.Empty) return BadRequest();
            try
            {
                var deletedProfile = await _accountService.DeleteUserProfileAsync(request.UserId);
                return Ok(deletedProfile);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
    }
}
