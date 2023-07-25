using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using time_tracker_api.Data;
using time_tracker_api.Models;

namespace time_tracker_api.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController : Controller
    {
        private readonly TimeTrackerDbContext _timeTrackerDbContext;

        public UserController(TimeTrackerDbContext timeTrackerDbContext)
        {
            _timeTrackerDbContext = timeTrackerDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var employees = await _timeTrackerDbContext.Users.ToListAsync();

            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> addUser([FromBody] User userRequest)
        {
            await _timeTrackerDbContext.Users.AddAsync(userRequest);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok(userRequest);
        }
        
       
    }
}
