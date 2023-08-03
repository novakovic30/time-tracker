using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using time_tracker_api.Data;
using time_tracker_api.Models;

namespace time_tracker_api.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : Controller
    {
        private readonly TimeTrackerDbContext _timeTrackerDbContext;
        
        public UserController(TimeTrackerDbContext timeTrackerDbContext, IConfiguration configuration)
        {
            _timeTrackerDbContext = timeTrackerDbContext;
        }

        // GET: /users
        // Retrieve all users from the database
        [HttpGet, Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _timeTrackerDbContext.Users.ToListAsync();
            return Ok(users);
        }
         
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] User userRequest)
        {
            await _timeTrackerDbContext.Users.AddAsync(userRequest);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok(userRequest);
        }

        // GET: /users/GetById/{id}
        // Retrieve a user by their ID
        [HttpGet("GetById/{id}"), Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _timeTrackerDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: /user/GetByEmail/{email}
        // Retrieve a user by their Email
        [HttpPost("GetByEmail/{email}"), Authorize]
        public async Task<IActionResult> CheckCredentials(string email)
        {
            var user = await _timeTrackerDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> addUser([FromBody] User userRequest)
        {
            await _timeTrackerDbContext.Users.AddAsync(userRequest);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok(userRequest);
        }

    }
}
