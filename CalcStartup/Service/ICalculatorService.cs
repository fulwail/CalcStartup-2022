using CalcStartup.Models;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public interface ICalculatorService
    {
        void CalculateNormOfTime(ProjectWork projectWork);
        void CalculateTimeFund( Project project);
        void CalculateDepreciation(Resource resource);
        void CalculateMonthlyExpenses(Consumable consumable);
        List<ColumnTableViewModel> CalculateAnnualWorkTimeBalance(Project project);
        List<ColumnTableViewModel> CalculateProjectPrice(Project project);
        List<SemiVariableExpense> CalculateSemiVariableExpense(Project project);
        decimal CalculateProceeds(Project project);
    }
}
