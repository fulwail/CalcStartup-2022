using CalcStartup.Models;

namespace CalcStartup.ViewModels
{
    public class EditProjectViewModel
    {
        public Project Project { get; set; } 
        public List<DistrictCoefficient> DistrictCoefficients { get; set; }
        public List<TariffRate> TariffRates { get; set; }
    }
}
