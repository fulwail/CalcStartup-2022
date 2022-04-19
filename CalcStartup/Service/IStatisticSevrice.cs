using CalcStartup.Models;
using CalcStartup.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CalcStartup.Service
{
    public interface IExcelSevrice
    {
        ReportResult CreateExcelReport(ReportViewModel excelReportDto);
     
    }
}