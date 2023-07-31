using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using time_tracker_api.Data;

namespace time_tracker_api.Controllers
{
    [ApiController]
    [Route("/tasks")]
    public class TaskController : Controller
    {
        private readonly TimeTrackerDbContext _timeTrackerDbContext;

        public TaskController(TimeTrackerDbContext timeTrackerDbContext)
        {
            _timeTrackerDbContext = timeTrackerDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _timeTrackerDbContext.Users.ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] Models.Task taskRequest)
        {
            // Get the currently logged-in user's ID
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out int userId))
            {
                // Handle the case where the user ID cannot be parsed to an integer
                return BadRequest("Invalid user ID format.");
            }

            // Associate the task with the currently logged-in user
            taskRequest.UserId = userId;

            // Save the task to the database
            await _timeTrackerDbContext.Tasks.AddAsync(taskRequest);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok(taskRequest);
        }
    }
}
