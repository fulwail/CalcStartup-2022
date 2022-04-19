using CalcStartup.Domain;
using CalcStartup.Models;
using CalcStartup.Models.Enums;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public class NsiService : INsiService
    {
        private readonly ISqlContext _context;
        private DateTime Firstdate = new DateTime(DateTime.Now.Year, 1, 1);
        private DateTime LastDate = new DateTime(DateTime.Now.Year, 12, 31);
        public NsiService(ISqlContext context)
        {
           
            _context =context;
        }

        public int GetWorkDays(WorkWeekType workWeekType)
        {
            var holidays = ActualizeHoliday();
             var holidaysCount = holidays.Where(h => CheckHoliday(h.Date)).Distinct().Count();
            var totalDays = GetCalendarDays();
            int weeks = totalDays / 7;
            if (LastDate.DayOfWeek < Firstdate.DayOfWeek) weeks++;
            totalDays -= weeks * 2;
            totalDays -= holidaysCount;
            return totalDays + 1;
        }
        public List<Holiday> GetHolidays()
        {
            return _context.Holidays.OrderBy(x=>x.Date).ToList();
        }
        public int GetCalendarDays()
        {
            return (int)(LastDate - Firstdate).TotalDays;
        }


        private bool CheckHoliday(DateTime holiday)
        {
            return (holiday.Date >= Firstdate && holiday <= LastDate
                && holiday.DayOfWeek != DayOfWeek.Saturday
                && holiday.DayOfWeek != DayOfWeek.Sunday);
        }
        private List<Holiday> ActualizeHoliday()
        {
            var holidays = _context.Holidays.ToArray();
            foreach (var holiday in holidays.Where(x=>x.Date.Year!= DateTime.Now.Year))
            {
                var currentHoliday = holiday.Date;
                holiday.Date = new DateTime(DateTime.Now.Year, currentHoliday.Month, currentHoliday.Day);
            }
            return holidays.ToList();
        }
        public void CreateHoliday(CreateHolidayViewModel createHoliday)
        {
            var holiday = new Holiday
            {
                Id = Guid.NewGuid(),
                Name = createHoliday.Name,
                Date = createHoliday.Date
            };
            _context.Holidays.Add(holiday);
            _context.SaveChanges();
        }
        public void DeleteHoliday (Guid id)
        {
           var holiday = _context.Holidays.FirstOrDefault(x=>x.Id==id);
            if (holiday!=null)
                _context.Holidays.Remove(holiday);
            _context.SaveChanges();
        }

        public List<TariffRate> GetTariffRates()
        {
            return _context.TariffRates.OrderBy(x=>x.Name).ThenBy(x=>x.Rank).ToList();
        }
        public void CreateTariffRate(CreateTariffRateViewModel tariffRateViewModel)
        {
            var tariffRate = new TariffRate { 
                Id = Guid.NewGuid(),
                Name = tariffRateViewModel.Name,
                Rank = tariffRateViewModel.Rank,
                Salary = tariffRateViewModel.Salary
            };
            _context.TariffRates.Add(tariffRate);
            _context.SaveChanges();
        }
        public void DeleteTariffRate(Guid id)
        {    
            var tariffRate = _context.TariffRates.FirstOrDefault(x => x.Id == id);
            if (tariffRate != null)
                _context.TariffRates.Remove(tariffRate);
            _context.SaveChanges();
        }

        public List<DistrictCoefficient> GetDistrictCoefficient()
        {
            return _context.DistrictCoefficients.ToList();
        }
        public void CreateDistrictCoefficient(CreateDistrictCoefficientsViewModel coefficient)
        {
            var districtCoefficients = new DistrictCoefficient
            {
                Id = Guid.NewGuid(),
                Name = coefficient.Name,
                Value = coefficient.Value
            };
            _context.DistrictCoefficients.Add(districtCoefficients);
            _context.SaveChanges();
        }
        public void DeleteDistrictCoefficient(Guid id)
        {
            var coefficient = _context.DistrictCoefficients.FirstOrDefault(x => x.Id == id);
            if (coefficient != null)
                _context.DistrictCoefficients.Remove(coefficient);
            _context.SaveChanges();
        }
    }
}
