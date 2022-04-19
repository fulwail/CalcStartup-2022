namespace CalcStartup.Models
{
    public class ProjectPrice
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Profit { get; set; }
        public decimal Amount { get; set; }
        public decimal Nds { get; set; }
        public decimal AmountWithNds { get; set; }
        public decimal Proceeds { get; set; }
        public decimal Count { get; set; }
    }
}
