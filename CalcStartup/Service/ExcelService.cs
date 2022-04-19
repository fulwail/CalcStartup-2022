using CalcStartup.Extensions;
using CalcStartup.Models;
using CalcStartup.ViewModels;
using ClosedXML.Excel;


namespace CalcStartup.Service
{
    public class ExcelService : IExcelSevrice
    {
        public ReportResult CreateExcelReport(ReportViewModel excelReportDto)
        {
            using (var wb = new XLWorkbook())
            {
                var annualWorkTimeBalanceWorksheet = wb.Worksheets.Add("Годовой баланс рабочего времени");

                annualWorkTimeBalanceWorksheet.Style.Font.SetFontName("Times New Roman");

                var simpleHeaders = GetSimpleHeaderList();

                var annualWorkTimeBalanceTitle = $"Годовой баланс рабочего времени";

                if (excelReportDto.AnnualWorkTimeBalanceList.Any())
                {
                    ExcelCreatorHelper.CreateReportTitle(annualWorkTimeBalanceTitle, simpleHeaders.Count, annualWorkTimeBalanceWorksheet);

                    var reports = from r in excelReportDto.AnnualWorkTimeBalanceList
                                  select new
                                  {
                                      Number = r.Number,
                                      Name = r.Name,
                                      Value = r.Value,
                                  };

                    ExcelCreatorHelper.CreateReportTable(reports, simpleHeaders,
                        excelReportDto.AnnualWorkTimeBalanceList.Count, annualWorkTimeBalanceWorksheet);
                
                }
                else
                {
                    AddNoDataRowExcel(annualWorkTimeBalanceWorksheet);
                }

                annualWorkTimeBalanceWorksheet.Columns().AdjustToContents();
                var ProjectPriceWorksheet = wb.Worksheets.Add("Техн.-экономические показатели");

                ProjectPriceWorksheet.Style.Font.SetFontName("Times New Roman");

                var projectPriceTitle = $"Технико-экономические показатели";

                if (excelReportDto.ProjectPriceList.Any())
                {
                    ExcelCreatorHelper.CreateReportTitle(projectPriceTitle, simpleHeaders.Count, ProjectPriceWorksheet);

                    var reports = from r in excelReportDto.ProjectPriceList
                                  select new
                                  {
                                      Number = r.Number,
                                      Name = r.Name,
                                      Value = r.Value,
                                  };

                    ExcelCreatorHelper.CreateReportTable(reports, simpleHeaders,
                        excelReportDto.ProjectPriceList.Count, ProjectPriceWorksheet);
                }
                else
                {
                    AddNoDataRowExcel(ProjectPriceWorksheet);
                }

                ProjectPriceWorksheet.Columns().AdjustToContents();

                var SemiVariableExpense = GetSemiVariableExpenseHeaderList();

                var SemiVariableExpenseWorksheet = wb.Worksheets.Add("Условно-переменные расходы");

                SemiVariableExpenseWorksheet.Style.Font.SetFontName("Times New Roman");

                var SemiVariableExpenseWorksheetTitle = $"Условно-переменные расходы";

                if (excelReportDto.SemiVariableExpenseList.Any())
                {
                    ExcelCreatorHelper.CreateReportTitle(SemiVariableExpenseWorksheetTitle, simpleHeaders.Count, SemiVariableExpenseWorksheet);

                    var reports = from r in excelReportDto.SemiVariableExpenseList
                                  select new
                                  {
                                      Name = r.Name,
                                      ProductExpense = r.ProductExpense,
                                      MounthExpense = r.MounthExpense,
                                      YearExpense = r.YearExpense,
                                  };

                    ExcelCreatorHelper.CreateReportTable(reports, SemiVariableExpense,
                        excelReportDto.SemiVariableExpenseList.Count, SemiVariableExpenseWorksheet, isCalculateSumForColumns: true);
                }
                else
                {
                    AddNoDataRowExcel(SemiVariableExpenseWorksheet);
                }
                SemiVariableExpenseWorksheet.Columns().AdjustToContents();
                using (var ms = new MemoryStream())
                {             
                    wb.SaveAs(ms);                   
                    return new ReportResult()
                    {
                        ReportContent = ms.ToArray(),
                        FileName = $"{excelReportDto.ProjectName} {DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
        private List<string> GetSimpleHeaderList()
        {
            return new List<string>()
            {
                "№",
                "Наименование",
                "Значение"   
            };
        }
        private List<string> GetSemiVariableExpenseHeaderList()
        {
            return new List<string>()
            {
                "Наименование",
                "Один программный продукт",
                 "Месяц",
                  "Год"
            };
        }
        private void AddNoDataRowExcel(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = "Нет данных для формирования отчета";
            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("A1").Style.Font.FontSize = 12;
            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range("A1:E1").Row(1).Merge();
        }

    }
}
