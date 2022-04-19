using CalcStartup.Identity.Model;
using CalcStartup.Models;
using CalcStartup.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CalcStartup.Domain
{
    public class SqlContext : IdentityDbContext<User>, ISqlContext
    {
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectWork> ProjectsWorks { get; set; }
        public DbSet<TimeFund> TimeFunds { get; set; }
        public DbSet<TariffRate> TariffRates { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<DistrictCoefficient> DistrictCoefficients { get; set;}
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<Resource> Resources { get; set; }

        public DbSet<Consumable> Consumables { get; set; }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options) => Database.EnsureCreated();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectUser>()
           .HasKey(c => new { c.UserId, c.ProjectId });

            modelBuilder.Entity<Holiday>()
                .Property(holiday => holiday.Id)
                .IsRequired();

            modelBuilder.Entity<Project>(x => {   
                x.Property(project => project.Id).IsRequired();
                x.HasMany(project => project.ProjectWorks)
                .WithOne(projectWork => projectWork.Project)
                .HasForeignKey(e => e.ProjectId);
                x.HasMany(c => c.ProjectUsers)
                .WithOne(x => x.Project);
            });

            modelBuilder.Entity<ProjectWork>()
                .Property(projectWork => projectWork.Id)
                .IsRequired();

            modelBuilder.Entity<TimeFund>()
                .Property(timeFund => timeFund.Id)
                .IsRequired();

            modelBuilder.Entity<TariffRate>()
                .Property(tariffRate => tariffRate.Id)
                .IsRequired();

            modelBuilder.Entity<DistrictCoefficient>()
                .Property(districtCoefficient => districtCoefficient.Id)
                .IsRequired();
            modelBuilder.Entity<Company>(x =>
            {
                x.Property(company => company.Id).IsRequired();
                x.HasMany(company => company.Projects)
                 .WithOne(Project => Project.Company);
            });
            modelBuilder.Entity<User>(x => {
                x.Property(project => project.Id).IsRequired();
                x.HasMany(project => project.ProjectUsers)
                .WithOne(x => x.User);
            });


        }
    }
}
