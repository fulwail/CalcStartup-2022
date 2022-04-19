namespace CalcStartup.ViewModels;

public class CreateConsumableViewModel
{
    public string Name { get; set; }
    public int Count { get; set; }
    public decimal Amount { get; set; }
    public Guid ProjectId { get; set; }
}
