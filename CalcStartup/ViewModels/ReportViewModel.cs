using CalcStartup.Models;

namespace CalcStartup.ViewModels
{
    public class ReportViewModel
    {
      public string ProjectName { get; set; }
      public  List<SemiVariableExpense> SemiVariableExpenseList { get; set; }
      public List<ColumnTableViewModel> ProjectPriceList { get; set; }
      public List<ColumnTableViewModel> AnnualWorkTimeBalanceList { get; set; }
      public Guid ProjectId { get; set; }

    }
}
