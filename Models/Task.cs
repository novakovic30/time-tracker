namespace time_tracker_api.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; }
        public bool Status { get; set; }
        public int TotalHours { get; set; }
        public int Hours { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
