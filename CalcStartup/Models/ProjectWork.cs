namespace CalcStartup.Models
{
    public class ProjectWork
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalTime { get; set; }
        public decimal PreparatoryFinalTime { get; set; }
        public decimal RestTime { get; set; }
        public decimal PersonalTime { get; set; }
        public decimal NormTime { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}

