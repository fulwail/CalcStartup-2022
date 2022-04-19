using System.ComponentModel.DataAnnotations;

namespace CalcStartup.ViewModels
{
    public class CreateHolidayViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set;}
    }
}
