namespace CalcStartup.Models
{
    public class Staff
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid TariffRateId { get; set; }
        public TariffRate TariffRate { get; set; }
        public int StaffCount { get; set; }
    }
}
