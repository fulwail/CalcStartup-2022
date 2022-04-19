using CalcStartup.Models;
using CalcStartup.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalcStartup.Domain
{
    public interface ISqlContext : IDisposable
    {
        DbSet<Holiday> Holidays { get; set; }
        DbSet<TariffRate> TariffRates { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<ProjectWork> ProjectsWorks { get; set; }
        DbSet<TimeFund> TimeFunds { get; set; }
        DbSet<ProjectUser> ProjectUsers { get; set; }
        DbSet<Staff> Staffs { get; set; }
        DbSet<DistrictCoefficient> DistrictCoefficients { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<Consumable> Consumables { get; }

        int SaveChanges();
    }
}
