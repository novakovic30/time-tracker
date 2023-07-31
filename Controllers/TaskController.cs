﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> getAllTasks()
        {
            var tasks = await _timeTrackerDbContext.Tasks.ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> addTask([FromBody] Models.Task taskRequest)
        {
            await _timeTrackerDbContext.Tasks.AddAsync(taskRequest);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok(taskRequest);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> getTasksById(int id)
        {
            var tasks = await _timeTrackerDbContext.Tasks
                .Where(task => task.UserId == id)
                .ToListAsync();

            if (!tasks.Any())
                return NotFound();

            return Ok(tasks);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> deleteTask(int id)
        {
            var task = await _timeTrackerDbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id);

            if(task == null)
                return NotFound();
            
            _timeTrackerDbContext.Tasks.Remove(task);
            await _timeTrackerDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
