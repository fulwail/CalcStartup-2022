using System.ComponentModel.DataAnnotations;

namespace CalcStartup.ViewModels
{
    public class CreateTariffRateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Rank { get; set;}
        [Required]
        public decimal Salary { get; set; }
    }
}
