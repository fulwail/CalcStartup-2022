namespace CalcStartup.Models
{
    public class Consumable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }  
        public decimal Amount { get; set; }
        public decimal Sum { get; set; }
        public Guid ProjectId { get; set; }
    }
}
