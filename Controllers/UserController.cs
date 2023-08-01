﻿using Microsoft.AspNetCore.Mvc;
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

        public UserController(TimeTrackerDbContext timeTrackerDbContext)
        {
            _timeTrackerDbContext = timeTrackerDbContext;
        }

        // GET: /users
        // Retrieve all users from the database
        [HttpGet]
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
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _timeTrackerDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: /users/CheckCredentials
        // Check if the provided login credentials are correct
        [HttpPost("CheckCredentials")]
        public async Task<IActionResult> CheckCredentials([FromBody] CredentialsModel credentials)
        {
            Console.Write(credentials.email + " " + credentials.password);
            var user = await _timeTrackerDbContext.Users.FirstOrDefaultAsync(u => u.Email == credentials.email);
            if (user == null)
            {
                return Ok(null);
            }
            if (user.Password == credentials.password)
            {
                return Ok(user);
            }
            return Ok(null);
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

    // Data model for login credentials
    public class CredentialsModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
