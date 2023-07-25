using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> addTask([FromBody] Models.Task taskRequest)
        {
            await _timeTrackerDbContext.Tasks.AddAsync(taskRequest);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok(taskRequest);
        }
    }
}
