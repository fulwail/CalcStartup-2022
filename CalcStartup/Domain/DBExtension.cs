using CalcStartup.Identity.Model;
using CalcStartup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CalcStartup.Domain
{
    public class DBExtension
    {
        public static async Task MigrateAsync(SqlContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            if (!context.UserRoles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Role.GlobalAdmin));
                await roleManager.CreateAsync(new IdentityRole(Role.TeamManager));
                await roleManager.CreateAsync(new IdentityRole(Role.Employee));
            }

            if (!context.Users.Any())
            {
                var administrator = new User
                {
                    UserName = "Admin"
                };
                await userManager.CreateAsync(administrator, "12345");
                await userManager.AddToRoleAsync(administrator, Role.GlobalAdmin);
            }
            if (!context.Holidays.Any())
            {
                var holidays = CreateDefaultHolidays();
                context.Holidays.AddRange(holidays);
               await context.SaveChangesAsync();
            }
            if (!context.TariffRates.Any())
            {
                var tariffRate = CreateDefaultTariffRates();
                context.TariffRates.AddRange(tariffRate);
                await context.SaveChangesAsync();
            }
            if(!context.DistrictCoefficients.Any())
            {
                var districtCoefficient =  CreateDefaultDistrictCoefficients();
                context.DistrictCoefficients.AddRange(districtCoefficient);
                await context.SaveChangesAsync();
            }
        }
        private static List<Holiday> CreateDefaultHolidays()
        {
            var currentYear = DateTime.Now.Year;
            var holidays = new List<Holiday>();
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 1) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 2) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 3) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 4) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 5) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 6) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Рождество Христово", Date = new DateTime(currentYear, 1, 7) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Новогодние каникулы", Date = new DateTime(currentYear, 1, 8) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "День защитника Отечества", Date = new DateTime(currentYear, 2, 23) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Международный женский день", Date = new DateTime(currentYear, 3, 8) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "Праздник Весны и Труда", Date = new DateTime(currentYear, 5, 1) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "День Победы", Date = new DateTime(currentYear, 5, 9) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "День России", Date = new DateTime(currentYear, 6, 12) });
            holidays.Add(new Holiday() { Id = Guid.NewGuid(), Name = "День народного единства", Date = new DateTime(currentYear, 11, 4) });
            return holidays;
        }
        private static List<TariffRate> CreateDefaultTariffRates()
        {
            var rates = new List<TariffRate>();
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Техник 2 кат.",
                Rank = 5,
                Salary = 1212.12m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Техник 1 кат.",
                Rank = 6,
                Salary = 1345.32m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Техник 1 кат.",
                Rank = 7,
                Salary = 1478.52m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Техник 1 кат.",
                Rank = 8,
                Salary = 1625.04m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Электроник",
                Rank = 7,
                Salary = 1478.52m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Электроник 2 кат.",
                Rank = 9,
                Salary = 1784.88m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Электроник 1 кат.",
                Rank = 10,
                Salary = 1958.04m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Электроник 1 кат.",
                Rank = 11,
                Salary = 2144.52m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Ведущий электроник",
                Rank = 13,
                Salary = 2504.16m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Программист",
                Rank = 7,
                Salary = 1478.52m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Программист 2 кат.",
                Rank = 8,
                Salary = 2317.66m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Программист 2 кат.",
                Rank = 9,
                Salary = 1784.88m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Программист 1 кат.",
                Rank = 11,
                Salary = 2144.52m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Программист 1 кат.",
                Rank = 10,
                Salary = 1958.04m
            });
            rates.Add(new TariffRate()
            {
                Id = Guid.NewGuid(),
                Name = "Ведущий программист",
                Rank = 13,
                Salary = 2504.16m
            });
            return rates;
        }
        private static List<DistrictCoefficient> CreateDefaultDistrictCoefficients()
        {
            var districtCoefficient = new List<DistrictCoefficient>();
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id= Guid.NewGuid(),
                Name= "Пермь",
                Value=1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Барнаул",
                Value = 1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Екатеринбург",
                Value = 1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Курган",
                Value = 1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Новосибирск",
                Value = 1.2m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Уфа",
                Value = 1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Стерлитамак",
                Value = 1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Красноярск",
                Value = 1.2m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Кемерово",
                Value = 1.3m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Омск",
                Value = 1.15m
            });
            districtCoefficient.Add(new DistrictCoefficient()
            {
                Id = Guid.NewGuid(),
                Name = "Челябинск",
                Value = 1.15m
            });
            return districtCoefficient;
        }
    }
   
}

