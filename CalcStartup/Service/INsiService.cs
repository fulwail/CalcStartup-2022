using CalcStartup.Models;
using CalcStartup.Models.Enums;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public interface INsiService
    {
        List<Holiday> GetHolidays();
        int GetWorkDays(WorkWeekType workWeekType);
        int GetCalendarDays();
        void DeleteHoliday(Guid id);
        void CreateHoliday(CreateHolidayViewModel createHoliday);
        void DeleteTariffRate(Guid id);
        void CreateTariffRate(CreateTariffRateViewModel tariffRateViewModel);
        List<TariffRate> GetTariffRates();
        List<DistrictCoefficient> GetDistrictCoefficient();
        void CreateDistrictCoefficient(CreateDistrictCoefficientsViewModel coefficient);
        void DeleteDistrictCoefficient(Guid id);
    }
}
