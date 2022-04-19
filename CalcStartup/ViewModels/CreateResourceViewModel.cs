using CalcStartup.Models.Enums;

namespace CalcStartup.ViewModels
{
    public class CreateResourceViewModel
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public decimal LifeTime { get; set; }
        public Guid ProjectId { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
