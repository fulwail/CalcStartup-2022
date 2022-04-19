using CalcStartup.Domain;
using CalcStartup.Models;
using CalcStartup.Models.Enums;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public class CalculatorService : ICalculatorService
    {
        private static decimal OverfulfillmentFactor = 1.1m;
        private static int Vacation = 28;
        private static int ExtrasalaryPercent = 10;
        private static int PremiumPercent = 30;
        private static int NdsPercent = 18;
        private readonly INsiService _nsiService;


        public CalculatorService(INsiService nsiService)
        {
            _nsiService = nsiService;
        }

        public void CalculateNormOfTime(ProjectWork projectWork)
        {
            projectWork.Id = Guid.NewGuid();
            projectWork.PreparatoryFinalTime = projectWork.TotalTime / 100 * 5;
            projectWork.RestTime = projectWork.TotalTime / 100 * 2;
            projectWork.PersonalTime = projectWork.TotalTime / 100 * 5;
            projectWork.NormTime = projectWork.TotalTime + projectWork.PreparatoryFinalTime + projectWork.RestTime + projectWork.PersonalTime;
        }
        public void CalculateTimeFund(Project project)
        {
            var worksDay = _nsiService.GetWorkDays(project.TimeFund.WorkWeekType);
            var normTime =( project.ProjectWorks != null&& project.ProjectWorks?.Count()!=0 )? project.ProjectWorks.Sum(x => x.NormTime) : 1m;
            project.TimeFund.MaxTimeFund = worksDay * project.TimeFund.WorkTime;
            project.TimeFund.PossibleYearTimeFund = project.TimeFund.MaxTimeFund * OverfulfillmentFactor;
            project.TimeFund.PossibleMonthTimeFund = project.TimeFund.PossibleYearTimeFund / 12;
            project.TimeFund.YearWorkQuantity = project.TimeFund.PossibleYearTimeFund / normTime;
            project.TimeFund.MonthWorkQuantity = project.TimeFund.PossibleMonthTimeFund / normTime;
        }

            public void CalculateDepreciation(Resource resource)
        {
            var sum = 0m;
            resource.Id = Guid.NewGuid();
            switch (resource.PaymentType)
            {      
                case PaymentType.Monthly:
                    sum = resource.Amount * resource.Count * resource.LifeTime / 12;
                    break;
                case PaymentType.Yearly:
                    sum = resource.Amount * resource.Count * resource.LifeTime;
                    break;
                case PaymentType.Single: 
                    sum = resource.Amount * resource.Count;
                    break;
            }
            resource.Sum=sum;
            resource.DepreciationRates = 100 / resource.LifeTime;
            resource.DepreciationCharges = resource.Sum * resource.DepreciationRates / 100;
        }
        public void CalculateMonthlyExpenses(Consumable consumable)
        {
            consumable.Sum = consumable.Amount * consumable.Count;
        }

        public List<ColumnTableViewModel> CalculateAnnualWorkTimeBalance(Project project)
        {
            var calendarDays = _nsiService.GetCalendarDays();
            var workDays = _nsiService.GetWorkDays(project?.TimeFund?.WorkWeekType?? WorkWeekType.day5);
            var columList = new List<ColumnTableViewModel>();
            columList.Add(new ColumnTableViewModel
            {
                Number = 1,
                Name = "Календарный фонд рабочего времени",
                Value = calendarDays.ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 2,
                Name = "Выходные и праздничные дни",
                Value = (calendarDays - workDays).ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 3,
                Name = "Рабочие дни",
                Value = workDays.ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 4,
                Name = "Общее количество плановых невыходов на работу",
                Value = (workDays / 100 + workDays / 100 + Vacation).ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 5,
                Name = "Отпуска",
                Value = Vacation.ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 6,
                Name = "По болезни",
                Value = (workDays / 100).ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 7,
                Name = "Прочие невыходы (учебные отпуска, выполнение административных обязанностей)",
                Value = (workDays / 100).ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 8,
                Name = "Плановый фонд рабочего времени",
                Value = (workDays - (workDays / 100 + workDays / 100 + Vacation)).ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 9,
                Name = "Номинальная продолжительность рабочего дня",
                Value = project?.TimeFund?.WorkTime.ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 10,
                Name = "Плановые сокращения рабочего дня",
                Value = "0.2"
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 11,
                Name = "Плановая продолжительность рабочего времени",
                Value = (project?.TimeFund?.WorkTime - 0.2m).ToString() ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 12,
                Name = "Плановая продолжительность рабочего времени за год",
                Value = ((workDays - (workDays / 100 + workDays / 100 + Vacation)) * (project?.TimeFund?.WorkTime - 0.2m))?.ToString("#,0.00#") ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 13,
                Name = "Месячный фонд времен",
                Value = ((workDays - (workDays / 100 + workDays / 100 + Vacation)) * (project?.TimeFund?.WorkTime - 0.2m) / 12)?.ToString("#,0.00#") ?? string.Empty
            });
            return columList;
        }

        public List<SemiVariableExpense> CalculateSemiVariableExpense(Project project)
        {
            var semiVariableExpensesList = new List<SemiVariableExpense>();
            var staffsSalary = 0m;
            if (project.Staffs != null)
            {
                foreach (var staf in project.Staffs)
                {
                    var pieceworkWages = staf?.TariffRate?.Salary * project?.ProjectWorks?.Sum(x => x.NormTime) ?? 1m;
                    var premium = pieceworkWages * PremiumPercent / 100;
                    var salary = pieceworkWages + premium;
                    var additionallySalary = ExtrasalaryPercent * (salary / 100);
                    salary += additionallySalary;
                    staffsSalary += salary * project?.DistrictCoefficient?.Value ?? 1m * staf?.StaffCount??1m;
                }
            }
            var wages = new SemiVariableExpense
            {
                Name = "Оплата труда",
                MounthExpense = Math.Round(staffsSalary,2),
                YearExpense = Math.Round(staffsSalary * 12,2),
                ProductExpense = Math.Round(staffsSalary / project?.TimeFund?.YearWorkQuantity ?? 1m,2)
            };
            semiVariableExpensesList.Add(wages);
            if (project.Resources != null)
            {
                var depreciation = new SemiVariableExpense
                {
                    Name = "Основные и вспомогательные материалы",
                    MounthExpense =Math.Round(( project?.Resources?.Sum(x => x.Sum) ?? 1m / 12),2),
                    YearExpense = Math.Round((project?.Resources?.Sum(x => x.Sum) ?? 1m), 2),
                    ProductExpense = Math.Round((project?.Resources?.Sum(x => x.Sum) ?? 1m / project?.TimeFund?.YearWorkQuantity ?? 1m),2)
                };
                semiVariableExpensesList.Add(depreciation);

            }
            return semiVariableExpensesList;
        }


            public decimal CalculateProceeds (Project project) {

            var semiVariableExpense = CalculateSemiVariableExpense(project).Sum(x => x.ProductExpense);
            var semiFixedExpense = project?.Consumables?.Sum(x => x.Sum);
            var service = semiVariableExpense + semiFixedExpense;
            var pribil = service / 100 * project.ProfitPercentage;
            var amount = pribil + service;
            return (project?.TimeFund?.YearWorkQuantity * amount) ?? 1m;

        }

        public List<ColumnTableViewModel> CalculateProjectPrice(Project project)
        {
            var columList = new List<ColumnTableViewModel>();
            var semiVariableExpense = CalculateSemiVariableExpense(project).Sum(x => x.ProductExpense);
            var semiFixedExpense = project?.Consumables?.Sum(x => x.Sum);
            var service = semiVariableExpense + semiFixedExpense;
            var pribil = service / 100 * project.ProfitPercentage;
            var amount = pribil + service;

            var p1 = project?.TimeFund?.PossibleYearTimeFund * amount;
            columList.Add(new ColumnTableViewModel
            {
                Number = 1,
                Name = "Выручка за год без НДС",
                Value = p1?.ToString("#,0.00#") ?? string.Empty
            });

            columList.Add(new ColumnTableViewModel
            {
                Number = 2,
                Name = "Переменные расходы (за год)",
                Value =semiVariableExpense.ToString("#,0.00#")
            });
            var p3 = p1 - semiVariableExpense;
            columList.Add(new ColumnTableViewModel
            {
                Number = 3,
                Name = "Маржинальный доход",
                Value = p3?.ToString("#,0.00#") ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 4,
                Name = "Постоянные расходы (за год)",
                Value = semiFixedExpense?.ToString(("#,0.00#") ?? string.Empty
            )});

            var p5 = p3 - semiFixedExpense;
            columList.Add(new ColumnTableViewModel
            {
                Number = 5,
                Name = "Прибыль от продаж",
                Value = p5?.ToString("#,0.00#" ?? string.Empty
            )});

            var p6 = project?.Resources?.Sum(x => x.Sum) / 100m * 2.2m;
            columList.Add(new ColumnTableViewModel
            {
                Number = 6,
                Name = "Налог на имущество",
                Value = p6?.ToString("#,0.00#") ?? string.Empty
            });
            var p7 = p5 - p6;
            columList.Add(new ColumnTableViewModel
            {
                Number = 7,
                Name = "Прибыль до налогообложения",
                Value = p7?.ToString("#,0.00#") ?? string.Empty
            });
            var p8 = p7 / 100 * 20;
            columList.Add(new ColumnTableViewModel
            {
                Number = 8,
                Name = "Налог на прибыль",
                Value = p8?.ToString("#,0.00#") ?? string.Empty
            });
            var p9 = p7 - p8;
            columList.Add(new ColumnTableViewModel
            {
                Number = 9,
                Name = "Чистая прибыль",
                Value = p9?.ToString("#,0.00#") ?? string.Empty
            });
            var p10 = p9 / p1 * 100;
            columList.Add(new ColumnTableViewModel
            {
                Number = 10,
                Name = "Рентабельность продукции",
                Value = p10?.ToString("#,0.00#") ?? string.Empty
            });

            columList.Add(new ColumnTableViewModel
            {
                Number = 11,
                Name = "Цена единицы продукции (без НДС)",
                Value = amount?.ToString("#,0.00#") ?? string.Empty
            });

            var ndsValue = amount / 100 * NdsPercent;
            columList.Add(new ColumnTableViewModel
            {
                Number = 12,
                Name = "НДС",
                Value = ndsValue?.ToString("#,0.00#") ?? string.Empty
            });

            var amountWithNds = amount + ndsValue;
            columList.Add(new ColumnTableViewModel
            {
                Number = 12,
                Name = "Цена единицы продукции (c НДС)",
                Value = ndsValue?.ToString("#,0.00#") ?? string.Empty
            });
            columList.Add(new ColumnTableViewModel
            {
                Number = 13,
                Name = "Условно-переменные затраты на единицу продукции",
                Value = semiFixedExpense?.ToString("#,0.00#") ?? string.Empty
            });
            var p14 = (semiVariableExpense / (amount - semiFixedExpense))*10;
            columList.Add(new ColumnTableViewModel
            {
                Number = 14,
                Name = "Точка безубыточности",
                Value = p14?.ToString("#,0.00#") ?? string.Empty
            });
            var p15 = p1 * semiFixedExpense / p3;
            columList.Add(new ColumnTableViewModel
            {
                Number = 15,
                Name = "Порог рентабельности",
                Value = p15?.ToString("#,0.00#") ?? string.Empty
            });
            var p16 = project?.Consumables?.Sum(x => x.Sum);
            columList.Add(new ColumnTableViewModel
            {
                Number = 16,
                Name = "Стоимость основных средств",
                Value = p16?.ToString("#,0.00#") ?? string.Empty
            });
            var p19 = p9 / p16??1m;
            columList.Add(new ColumnTableViewModel
            {
                Number = 17,
                Name = "Затраты на создание проекта",
                Value = p19.ToString("#,0.00#") ?? string.Empty
            });
            var p20 = 1 / p19;
            columList.Add(new ColumnTableViewModel
            {
                Number = 17,
                Name = "Фактический срок окупаемости, лет",
                Value = p20.ToString("#,0.00###") ?? string.Empty
            });

            return columList;
        }
    }
}
