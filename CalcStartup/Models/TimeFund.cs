using CalcStartup.Models.Enums;

namespace CalcStartup.Models
{
    public class TimeFund 
    {
        public Guid Id { get; set; }
        public decimal WorkTime { get; set; }
        public decimal MaxTimeFund { get; set; }
        public decimal PossibleYearTimeFund { get; set; }
        public decimal PossibleMonthTimeFund { get; set; }
        public decimal YearWorkQuantity { get; set; }
        public decimal MonthWorkQuantity { get; set; }
        public WorkWeekType WorkWeekType { get; set; }  
    }
}
