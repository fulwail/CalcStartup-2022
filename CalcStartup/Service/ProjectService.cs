using CalcStartup.Domain;
using CalcStartup.ViewModels;
using CalcStartup.Models;
using Microsoft.AspNetCore.Identity;
using CalcStartup.Identity.Model;
using Microsoft.EntityFrameworkCore;

namespace CalcStartup.Service
{
    public class ProjectService : IProjectService
    {
        private readonly ISqlContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ICalculatorService _calculatorService;
        public ProjectService(ISqlContext context, UserManager<User> userManager, ICalculatorService calculatorService)
        {
            _context = context;
            _userManager = userManager;
            _calculatorService = calculatorService;
        }

        public void AddUser(Guid UserId, Guid ProjectId)
        {
            if (!_context.ProjectUsers.Any (x => x.ProjectId == ProjectId && x.UserId == UserId.ToString()))
                _context.ProjectUsers.Add(new ProjectUser { UserId = UserId.ToString(), ProjectId = ProjectId });
            _context.SaveChanges();

        }

        public void Create(CreateProjectViewModel createProjectView, string userId)
        {
            var project = new Project() 
            {
                Id = Guid.NewGuid(),
                CompanyId = createProjectView.CompanyId,
                Name = createProjectView.Name            
            };
            _context.Projects.Add(project);
            _context.ProjectUsers.Add(new ProjectUser { UserId =userId, ProjectId = project.Id });
            _context.SaveChanges();
        }   

        public void Delete(Guid id)
        {
            var project = _context.Projects.FirstOrDefault(x => x.Id == id);
            if (project != null)
                _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        public void DeleteUser(Guid userId, Guid projectId)
        {
            var projectUsers = _context.ProjectUsers.FirstOrDefault(x => x.ProjectId == projectId && x.UserId==userId.ToString());
          
            if (projectUsers != null)
                _context.ProjectUsers.Remove(projectUsers);
            _context.SaveChanges();
        }

        public List<Project> GetProjectsByUser(User user, bool isAdmin)
        {
            var projects = new List<Project>();
            if (isAdmin)
                projects = _context.Projects.Where(x => x.CompanyId == user.CompanyId).ToList();
            else
            {
                projects= _context.ProjectUsers.Where(x=>x.UserId==user.Id).Select(x=>x.Project).ToList();
            }
            return projects;
        }

        public Project GetProjectById(Guid? Id)
        {
            return _context.Projects
                .Include(x => x.TimeFund)
                .Include(x=>x.Company)
                .Include(x=>x.Consumables)
                .Include(x => x.ProjectUsers)
                .Include(x => x.ProjectWorks)
                .Include(x => x.Resources)
                .Include(x => x.Staffs).ThenInclude(x=>x.TariffRate)
                .Include(x => x.DistrictCoefficient)
                .Where(x => x.Id == Id).FirstOrDefault(); 
        }

        public void Update(UpdateProjectViewModel project)
        {
          var currentProject = GetProjectById(project.Id);
            if (currentProject != null)
            {
                currentProject.Name=project.Name;
                currentProject.ProfitPercentage=project.ProfitPercentage;
                currentProject.DistrictCoefficientId = project.DistrictCoefficientId;
                _context.Projects.Update(currentProject);
                _context.SaveChanges();
            }
            if(currentProject.TimeFund == null)
            {
                currentProject.TimeFund = new TimeFund
                {
                    Id = Guid.NewGuid(),
                    WorkTime = project.TimeFund.WorkTime,
                    WorkWeekType = project.TimeFund.WorkWeekType,
                };
                _context.TimeFunds.Add(currentProject.TimeFund);
                _context.SaveChanges();

            } 
            else
            {
             currentProject.TimeFund.WorkTime = project.TimeFund.WorkTime;
             currentProject.TimeFund.WorkWeekType=project.TimeFund.WorkWeekType;
            }    
          
            _calculatorService.CalculateTimeFund(currentProject);
            _context.TimeFunds.Update(currentProject.TimeFund);
            _context.SaveChanges();
        }

        public void DeleteProjectWork(Guid id)
        {
            var projectsWorks = _context.ProjectsWorks.FirstOrDefault(x => x.Id == id);
            if (projectsWorks != null)
                _context.ProjectsWorks.Remove(projectsWorks);
            _context.SaveChanges();
        }

        public void CreateProjectWork(CreateProjectWorkViewModel createProjectView)
        {
            var projectsWork = new ProjectWork
            {
                Id = Guid.NewGuid(),
                Name = createProjectView.Name,
                TotalTime = createProjectView.TotalTime,
                ProjectId = createProjectView.ProjectId
            };
            _calculatorService.CalculateNormOfTime(projectsWork);
            _context.ProjectsWorks.Add(projectsWork);
            _context.SaveChanges();
        }
        public void CreateStaff(CreateStaffViewModel createProjectView)
        {
            var staff = new Staff
            {
                Id = Guid.NewGuid(),
                StaffCount = createProjectView.StaffCount,
                TariffRateId = createProjectView.TariffRateId,
                ProjectId = createProjectView.ProjectId
            };
            _context.Staffs.Add(staff);
            _context.SaveChanges();
        }

        public void CreateResource(CreateResourceViewModel createProjectView)
        {
            var resource = new Resource
            {
                Id = Guid.NewGuid(),
                Name = createProjectView.Name,
                Amount = createProjectView.Amount,
                Count = createProjectView.Count,
                ProjectId = createProjectView.ProjectId,
                LifeTime = createProjectView.LifeTime,
                PaymentType=createProjectView.PaymentType
            };
            _calculatorService.CalculateDepreciation(resource);
            _context.Resources.Add(resource);
            _context.SaveChanges();
        }

        public void DeleteStaff(Guid id)
        {
            var staff = _context.Staffs.FirstOrDefault(x => x.Id == id);
            if (staff != null)
                _context.Staffs.Remove(staff);
            _context.SaveChanges();
        }

        public void DeleteResource(Guid resourceId)
        {
            var resource = _context.Resources.FirstOrDefault(x => x.Id == resourceId);
            if (resource != null)
                _context.Resources.Remove(resource);
            _context.SaveChanges();
        }

        public void DeleteConsumable(Guid consumableId)
        {
            var consumable = _context.Consumables.FirstOrDefault(x => x.Id == consumableId);
            if (consumable != null)
                _context.Consumables.Remove(consumable);
            _context.SaveChanges();
        }

        public void CreateConsumable(CreateConsumableViewModel createProjectView)
        {
            var consumable = new Consumable
            {
                Id = Guid.NewGuid(),
                Name = createProjectView.Name,
                Amount = createProjectView.Amount,
                Count = createProjectView.Count,
                ProjectId = createProjectView.ProjectId,
            };
            _calculatorService.CalculateMonthlyExpenses(consumable);
            _context.Consumables.Add(consumable);
            _context.SaveChanges();
        }

        public ReportViewModel GetStatistics(Guid id)
        {
            var project =GetProjectById(id);
            var projectPriceList = _calculatorService.CalculateProjectPrice(project);
            var annualWorkTimeBalanceList = _calculatorService.CalculateAnnualWorkTimeBalance(project);
            var semiVariableExpenseList = _calculatorService.CalculateSemiVariableExpense(project);

            var report = new ReportViewModel
            {
                ProjectId = id,
                ProjectName = project.Name,
                ProjectPriceList = projectPriceList,
                AnnualWorkTimeBalanceList = annualWorkTimeBalanceList,
                SemiVariableExpenseList = semiVariableExpenseList
            };
            return report;
        }

        public ChartViewModelList GetBreakEvenVhart(Project project)
        {
            var graphData = new ChartViewModelList();
            var semiVariableExpenseList = _calculatorService.CalculateSemiVariableExpense(project);
          
            List<GraphDataSet> graphDataSets = new List<GraphDataSet>()
                {
                    new GraphDataSet()
                    {
                        LabelForDataSet =  "Условно-постоянные расходы",
                        DataForDataSet =new List<ScatterConfig>() {
                            new ScatterConfig()
                            {
                                X=0,
                                Y= project.Consumables.Sum(x=>x.Sum)*10,
                            },
                              new ScatterConfig()
                            {
                                X=project.TimeFund.YearWorkQuantity,
                                Y= project.Consumables.Sum(x=>x.Sum)*10,
                            }
                        },
                        BorderColorForDataSet = "rgb(252, 3, 3)",
                        IsFilled = false,
                    },
                    new GraphDataSet()
                    {
                        LabelForDataSet = "Условно-переменные расходы",
                       DataForDataSet =new List<ScatterConfig>() {
                            new ScatterConfig()
                            {
                                X=0,
                                Y=    project.Consumables.Sum(x => x.Sum)*10,
                            },
                              new ScatterConfig()
                            {
                                X=project.TimeFund.YearWorkQuantity,
                                Y=   ((project.Consumables.Sum(x => x.Sum)*10)+(semiVariableExpenseList.Sum(x => x.ProductExpense)/17))
                            },
                        },
                        BorderColorForDataSet = "rgb(3, 252, 49)",
                        IsFilled = false,
                    },
                    new GraphDataSet()
                    {
                        LabelForDataSet ="Выручка",
                         DataForDataSet =new List<ScatterConfig>() {
                            new ScatterConfig()
                            {
                                X=0,
                                Y=0,
                            },
                              new ScatterConfig()
                            {
                                X=project.TimeFund.YearWorkQuantity,
                                Y= _calculatorService.CalculateProceeds(project)/120,
                            },
                        },
                        BorderColorForDataSet = "rgb(24, 3, 252)",
                        IsFilled = false,
                    },
                 
                };
            graphData.DataSets = graphDataSets;
        
            return graphData;
        }
     
    }
}
