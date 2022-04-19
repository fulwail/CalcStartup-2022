using CalcStartup.Models.Enums;

namespace CalcStartup.Models
{
    public class Resource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public decimal Sum { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal LifeTime { get; set; }
        public decimal DepreciationRates { get; set; }
        public decimal DepreciationCharges { get; set; }
        public Guid ProjectId { get; set; }
    }
}
