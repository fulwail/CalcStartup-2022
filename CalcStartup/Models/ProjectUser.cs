using CalcStartup.Identity.Model;

namespace CalcStartup.Models
{
    public class ProjectUser
    {
        public Guid ProjectId { get; set; }
        public Project  Project { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}