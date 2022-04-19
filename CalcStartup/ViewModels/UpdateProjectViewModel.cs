using CalcStartup.Models.Enums;

namespace CalcStartup.ViewModels
{
    public class UpdateProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? ProfitPercentage { get; set; }
        public TimeFundViewModel TimeFund { get; set; }     
        public Guid DistrictCoefficientId { get; set; }
    }
}
