using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using time_tracker_api.Data;
using time_tracker_api.Models;

namespace time_tracker_api.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly TimeTrackerDbContext _timeTrackerDbContext;

        public AuthController(TimeTrackerDbContext timeTrackerDbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _timeTrackerDbContext = timeTrackerDbContext;
        }

        // POST: /auth/CheckCredentials
        // Check if the provided login credentials are correct and return token
        [HttpPost("CheckCredentials")]
        public async Task<IActionResult> CheckCredentials([FromBody] CredentialsModel credentials)
        {
            Console.Write(credentials.email + " " + credentials.password);
            var user = await _timeTrackerDbContext.Users.FirstOrDefaultAsync(u => u.Email == credentials.email);
            if (user == null)
            {
                return Ok(null);
            }
            if (user.Password != credentials.password)
            {
                return Ok(null);
            }
            string token = GenerateJSONWebToken(user);
            return Ok(token);
        }

        // GET: /auth/CheckToken
        [HttpGet("CheckToken"), Authorize]
        public async Task<IActionResult> checkToken()
        {
            String email = User.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                var user = await _timeTrackerDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return BadRequest("user not found.");
                }
                return Ok(user);
            }
            return BadRequest("email not found.");
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            List<Claim> claims = new List<Claim> {
                 new Claim(JwtRegisteredClaimNames.Sub, userInfo.LastName),
                 new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Data model for login credentials
    public class CredentialsModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
