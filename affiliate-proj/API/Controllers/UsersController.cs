using affiliate_proj.Application.Interfaces;
using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers
{
    // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        private static List<User> users = new List<User>
        {
            new User
            {
                UserId = new Guid("7ad805da-0f53-41a7-b1cf-72e95b737dd2"),
                Username = "admin",
                Password = "admin",
                CreatedAt = DateTime.Now
            },
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "user",
                Password = "user",
                CreatedAt = DateTime.Now
            },
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "user2",
                Password = "user2",
                CreatedAt = DateTime.Now
            },
            new User
            {
                UserId = new Guid("5233d2b1-d7a6-4113-8500-6a7cce80c258"),
                Username = "user3",
                Password = "user3",
                CreatedAt = DateTime.Now
            }
        };

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var returnedUsers = await _accountService.GetAllUsersAsync();
            // return Ok(users);
            return Ok(returnedUsers);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(System.Guid userId)
        {
            // var user =  users.FirstOrDefault(x => x.UserId == userId);
            var user = await _accountService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();
            
            // return Ok(users.Find(u => u.uuid == uuid));
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> AddUser(User user)
        {
            if (user == null) return BadRequest();
            if (users.FirstOrDefault(x => x.UserId == user.UserId) != null) return BadRequest();
            
            user.CreatedAt = DateTime.Now;
            users.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { uuid = user.UserId }, user);
        }

        [HttpPut("{uuid}")]
        public IActionResult UpdateUser(System.Guid uuid, User user)
        {   
            if (uuid != user.UserId) return NotFound();

            var selectedUser =  users.FirstOrDefault(x => x.UserId == uuid);
            
            selectedUser.Username = user.Username;
            selectedUser.Password = user.Password;

            return NoContent();
        }

        [HttpDelete("{uuid}")]
        public IActionResult DeleteUser(System.Guid uuid)
        {
            var selectedUser = users.FirstOrDefault(x => x.UserId == uuid);
            if (selectedUser == null) return NotFound();
            users.Remove(selectedUser);
            return NoContent();
        }

    }
}
