using ClosedXML.Excel;

namespace CalcStartup.Extensions
{
    public static class ExcelCreatorHelper
    {
        public static void CreateReportTitle(string title, int columnsCount, IXLWorksheet worksheet)
        {
            worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            worksheet.Cell("A1").Value = title;
            worksheet.Cell("A1").Style.Font.Bold = true;
            worksheet.Cell("A1").Style.Font.FontSize = 12;
            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range($"A1:{GetColumnNameByNumeric(columnsCount - 1)}1").Row(1).Merge();
            worksheet.Range($"A2:{GetColumnNameByNumeric(columnsCount - 1)}2").Row(1).Merge();
        }

        public static void CreateReportTable(IEnumerable<object> reportRows, List<string> headers, int rowsCount, IXLWorksheet worksheet , string countRecordsColumnName = "Всего:",
            bool isCalculateSumForColumns = false, bool isCreateFirstNumericRow = false)
        {
            var rowsList = reportRows.ToList();
            IXLTable table;

                table = worksheet.Cell( 4, 1).InsertTable(rowsList);

            foreach (var cell in table.AsRange().Cells().Where(x => x.DataType == XLDataType.DateTime))
                cell.Style.NumberFormat.Format = "dd.mm.yyyy";

         

            table.Columns().Style.Alignment.SetShrinkToFit(true);

            worksheet.AutoFilter.Clear();

            if (isCreateFirstNumericRow)
            {
                table.InsertRowsAbove(1, true);

                for (var i = 0; i < headers.Count; i++)
                {
                    var headerCell = worksheet.Cell(4, i + 1);
                    headerCell.SetValue(headers[i]);
                    headerCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    var numericCell = worksheet.Cell($"{GetColumnNameByNumeric(i)}5");
                    numericCell.SetValue(i + 1);
                    numericCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }
            }
            else
            {
                for (var i = 0; i < headers.Count; i++)
                {
                    var headerCell = worksheet.Cell( 4, i + 1);
                    headerCell.SetValue(headers[i]);
                    headerCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }
            }

            var tableRow =  rowsList.Count + 6;
          
            if (isCalculateSumForColumns)
            {
          
                var recordsCount = isCreateFirstNumericRow ? rowsCount +  7 : rowsCount + 6;
                var distanceToFirstRecord = isCreateFirstNumericRow ?  6 :  5;

                worksheet.Cell($"A{recordsCount}").Value = "Всего: ";

                for (int i = 0; i < headers.Count(); i++)
                {
                    var columnName = GetColumnNameByNumeric(i);

                    if (worksheet.Cell($"{columnName}{distanceToFirstRecord}").DataType == XLDataType.Number)
                    {
                        worksheet.Cell($"{columnName}{recordsCount}").Value =
                            worksheet.Evaluate($"SUM({columnName}{distanceToFirstRecord}:{columnName}{recordsCount})");
                    }
                }
            }
        }
        public static string GetColumnNameByNumeric(int index)
        {
            var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

    }
}
