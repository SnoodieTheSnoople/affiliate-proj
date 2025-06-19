using affiliate_proj.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace affiliate_proj.API.Controllers
{
    // [Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        static private List<User> users = new List<User>
        {
            new User
            {
                uuid = "7ad805da-0f53-41a7-b1cf-72e95b737dd2",
                username = "admin",
                password = "admin",
                created_at = DateTime.Now
            },
            new User
            {
                uuid = Guid.NewGuid().ToString(),
                username = "user",
                password = "user",
                created_at = DateTime.Now
            },
            new User
            {
                uuid = Guid.NewGuid().ToString(),
                username = "user2",
                password = "user2",
                created_at = DateTime.Now
            },
            new User
            {
                uuid = "5233d2b1-d7a6-4113-8500-6a7cce80c258",
                username = "user3",
                password = "user3",
                created_at = DateTime.Now
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
            var user =  users.FirstOrDefault(x => x.uuid == uuid);
            if (user == null) return NotFound();
            
            // return Ok(users.Find(u => u.uuid == uuid));
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> AddUser(User user)
        {
            if (user == null) return BadRequest();
            if (users.FirstOrDefault(x => x.uuid == user.uuid) != null) return BadRequest();
            
            user.created_at = DateTime.Now;
            users.Add(user);
            return CreatedAtAction(nameof(GetUserByUUID), new { uuid = user.uuid }, user);
        }

        [HttpPut("{uuid}")]
        public IActionResult UpdateUser(string uuid, User user)
        {   
            if (uuid != user.uuid) return NotFound();

            var selectedUser =  users.FirstOrDefault(x => x.uuid == uuid);
            
            selectedUser.username = user.username;
            selectedUser.password = user.password;

            return NoContent();
        }

        [HttpDelete("{uuid}")]
        public IActionResult DeleteUser(string uuid)
        {
            var selectedUser = users.FirstOrDefault(x => x.uuid == uuid);
            if (selectedUser == null) return NotFound();
            users.Remove(selectedUser);
            return NoContent();
        }

    }
}
