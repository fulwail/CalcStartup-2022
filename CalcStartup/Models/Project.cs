using CalcStartup.Identity.Model;

namespace CalcStartup.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public virtual List<ProjectUser> ProjectUsers { get; set; }
        public virtual List<ProjectWork> ProjectWorks { get; set; }
        public TimeFund TimeFund { get; set; }
        public Guid? TimeFundId { get; set; }
        public List<Staff> Staffs { get; set; }
        public DistrictCoefficient DistrictCoefficient { get; set; }
        public Guid? DistrictCoefficientId { get; set; }
        public List<Resource> Resources { get; set; }
        public List<Consumable> Consumables { get; set; }
        public decimal? ProfitPercentage { get; set; }
    }
}
