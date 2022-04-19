using CalcStartup.Domain;
using CalcStartup.Identity.Model;
using CalcStartup.Service;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System.Globalization;
using System.Reflection;
namespace CalcStartup
{
    public class Startup
    {
        private readonly  IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        { 
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddMvc(options =>
                 options.EnableEndpointRouting = false);
          
            services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                     sqlOptions => { sqlOptions.MigrationsAssembly(typeof(SqlContext).Assembly.GetName().Name); }
                 );
            });
            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
               .AddEntityFrameworkStores<SqlContext>();

            var config = new TypeAdapterConfig();
            config.Scan(Assembly.GetAssembly(typeof(Program)));


            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICalculatorService, CalculatorService>();
            services.AddTransient<INsiService, NsiService>();
            services.AddScoped<ISqlContext, SqlContext>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IExcelSevrice, ExcelService>();
        }
        public void Configure(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SqlContext>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                AsyncContext.Run(async () => { await DBExtension.MigrateAsync(context, roleManager, userManager); });
            }

            app.UseDeveloperExceptionPage();



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
                       app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    "default", "{controller=Project}/{action=Index}/{id?}");
            });  
        }
    }
}
