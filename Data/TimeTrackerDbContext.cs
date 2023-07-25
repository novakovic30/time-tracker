using Microsoft.EntityFrameworkCore;
using time_tracker_api.Models;

namespace time_tracker_api.Data
{
    public class TimeTrackerDbContext : DbContext
    {
        public TimeTrackerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Models.Task> Tasks { get; set; }
    }
}
