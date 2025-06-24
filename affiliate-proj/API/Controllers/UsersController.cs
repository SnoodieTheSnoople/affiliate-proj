using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers
{
    // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> users = new List<User>
        {
            new User
            {
                Uuid = "7ad805da-0f53-41a7-b1cf-72e95b737dd2",
                Username = "admin",
                Password = "admin",
                CreatedAt = DateTime.Now
            },
            new User
            {
                Uuid = Guid.NewGuid().ToString(),
                Username = "user",
                Password = "user",
                CreatedAt = DateTime.Now
            },
            new User
            {
                Uuid = Guid.NewGuid().ToString(),
                Username = "user2",
                Password = "user2",
                CreatedAt = DateTime.Now
            },
            new User
            {
                Uuid = "5233d2b1-d7a6-4113-8500-6a7cce80c258",
                Username = "user3",
                Password = "user3",
                CreatedAt = DateTime.Now
            }
        };

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(users);
        }

        [HttpGet("{uuid}")]
        public ActionResult<User> GetUserByUUID(string uuid)
        {
            var user =  users.FirstOrDefault(x => x.Uuid == uuid);
            if (user == null) return NotFound();
            
            // return Ok(users.Find(u => u.uuid == uuid));
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> AddUser(User user)
        {
            if (user == null) return BadRequest();
            if (users.FirstOrDefault(x => x.Uuid == user.Uuid) != null) return BadRequest();
            
            user.CreatedAt = DateTime.Now;
            users.Add(user);
            return CreatedAtAction(nameof(GetUserByUUID), new { uuid = user.Uuid }, user);
        }

        [HttpPut("{uuid}")]
        public IActionResult UpdateUser(string uuid, User user)
        {   
            if (uuid != user.Uuid) return NotFound();

            var selectedUser =  users.FirstOrDefault(x => x.Uuid == uuid);
            
            selectedUser.Username = user.Username;
            selectedUser.Password = user.Password;

            return NoContent();
        }

        [HttpDelete("{uuid}")]
        public IActionResult DeleteUser(string uuid)
        {
            var selectedUser = users.FirstOrDefault(x => x.Uuid == uuid);
            if (selectedUser == null) return NotFound();
            users.Remove(selectedUser);
            return NoContent();
        }

    }
}
