namespace CalcStartup.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Project> Projects { get; set; }
    }
}
