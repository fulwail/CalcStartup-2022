using System.ComponentModel.DataAnnotations;

namespace CalcStartup.ViewModels
{
    public class CreateDistrictCoefficientsViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Value { get; set;}
       
    }
}
