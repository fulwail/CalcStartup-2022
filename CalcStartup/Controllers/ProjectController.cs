using CalcStartup.Identity.Model;
using CalcStartup.Models;
using CalcStartup.Service;
using CalcStartup.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CalcStartup.Controllers
{

    public class ProjectController : Controller
    {
        private static string contentType = "application/octet-stream";
        private readonly ILogger<ProjectController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IProjectService _projectService;
        private readonly INsiService _nsiService;
        private readonly IExcelSevrice _excelService;
        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService, UserManager<User> userManager, INsiService nsiService, IExcelSevrice excelService)
        {
            _logger = logger;
            _projectService = projectService;
            _userManager = userManager;
            _nsiService = nsiService;
            _excelService = excelService;
        }



        [Authorize()]
        public IActionResult Index()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var projects = _projectService.GetProjectsByUser(user, User.IsInRole(Role.GlobalAdmin));
            var projectsList = projects.Select(x=> new ProjectViewModel 
            { 
                Id=x.Id,
                Name=x.Name    
            }).ToList();
            var projectsView = new ProjectListViewModel
            {
                CompanyId = user.CompanyId,
                Projects = projectsList
            };
            return View(projectsView);

        }

        [Authorize()]
        public FileContentResult DownoloadExcel(Guid ProjectId)
        {
            var statistics = _projectService.GetStatistics(ProjectId);
            var content= _excelService.CreateExcelReport(statistics);
            return File(content.ReportContent, contentType, content.FileName);

        }
        [HttpPost]
        public JsonResult CreateChart(Guid Id)
        {
            var project = _projectService.GetProjectById(Id);
            var chart = _projectService.GetBreakEvenVhart(project);
            string jsonGraphdata = JsonConvert.SerializeObject(chart);

            return new JsonResult(jsonGraphdata) { Value= jsonGraphdata };

        }

        [Authorize(Roles = Role.AllAdmin)]
        public IActionResult Team(Guid Id)
        {     
            var users = _userManager.Users.Where(x=>x.ProjectUsers.Any(p=>p.ProjectId== Id)).ToList();
            var usersViewModel = users.Select(x => new UserViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                RoleName = _userManager.GetRolesAsync(x).Result.FirstOrDefault()
            }).ToList();
            var projectView = new ProjectTeamsViewModel() { Users = usersViewModel, ProjectId = Id };
            return View(projectView);

        }
        [Authorize()]
        public IActionResult Edit(Guid Id)
        {
            var project = _projectService.GetProjectById(Id);
            var districtCoefficients = _nsiService.GetDistrictCoefficient();
            var tariffRates = _nsiService.GetTariffRates();
            var projevtView = new EditProjectViewModel
            {
                Project = project,
                DistrictCoefficients= districtCoefficients,
                TariffRates= tariffRates
            };
            return View(projevtView);

        }

        [Authorize()]
        public IActionResult Statistics(Guid Id)
        {
            var statistics = _projectService.GetStatistics(Id);
            return View(statistics);

        }

        [Authorize()]
        [HttpPost]
        public IActionResult UpdateProject(UpdateProjectViewModel project)
        {
             _projectService.Update(project);
            return RedirectToAction("Edit", new { Id = project.Id });

        }
        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public IActionResult Delete(Guid Id)
        {
            _projectService.Delete(Id);        
            return RedirectToAction("Index");
        }
        [Authorize()]
        [HttpPost]
        public IActionResult DeleteProjectWork(Guid ProjectWorkId, Guid ProjectId)
        {
            _projectService.DeleteProjectWork(ProjectWorkId);

            return RedirectToAction("Edit", new { Id = ProjectId });
        }


        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public IActionResult DeleteUser(Guid UserId,Guid Id)
        {
            _projectService.DeleteUser(UserId, Id);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = Role.AllAdmin)]
        public IActionResult AddUser(Guid ProjectId)
        {
            var projectTeams = new ProjectTeamsViewModel();
            var serialaseUsers = TempData["Users"] as string;
            if(serialaseUsers != null)
                projectTeams.Users = JsonConvert.DeserializeObject<List<UserViewModel>>(TempData["Users"] as string);      
           else
            {
                var project = _projectService.GetProjectById(ProjectId);
                var users = _userManager.Users.Where(x => x.CompanyId == project.CompanyId).ToList() ;    
                if (users.Count()!=0)
                    projectTeams.Users= users.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    RoleName = _userManager.GetRolesAsync(x).Result.First()
                }).ToList();
            }
          
            projectTeams.ProjectId = ProjectId; 
            return View(projectTeams);
        }
        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public IActionResult Add(Guid UserId, Guid Id)
        {
            _projectService.AddUser(UserId, Id);
           
            return RedirectToAction("Index");
        }
        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public IActionResult Create(CreateProjectViewModel createProjectView)
        {
            if (ModelState.IsValid)
            {
                _projectService.Create(createProjectView,User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else
            {
                TempData["Message"] = "Все поля обязательны для заполнения";
            }

            return RedirectToAction("Index");

        }

        [Authorize()]
        [HttpPost]
        public IActionResult CreateProjectWorks(CreateProjectWorkViewModel createProjectView)
        {
           _projectService.CreateProjectWork(createProjectView);

            return RedirectToAction("Edit", new { Id = createProjectView.ProjectId });

        }

        [Authorize()]
        [HttpPost]
        public IActionResult CreateStaff(CreateStaffViewModel createProjectView)
        {
            _projectService.CreateStaff(createProjectView);

            return RedirectToAction("Edit", new { Id = createProjectView.ProjectId });

        }
        [Authorize()]
        [HttpPost]
        public IActionResult DeleteStaff(Guid StaffId, Guid ProjectId)
        {
            _projectService.DeleteStaff(StaffId);

            return RedirectToAction("Edit", new { Id = ProjectId });
        }

        [Authorize()]
        [HttpPost]
        public IActionResult CreateResource(CreateResourceViewModel createProjectView)
        {
            _projectService.CreateResource(createProjectView);

            return RedirectToAction("Edit", new { Id = createProjectView.ProjectId });

        }

        [Authorize()]
        [HttpPost]
        public IActionResult DeleteResource(Guid ResoursceId, Guid ProjectId)
        {
            _projectService.DeleteResource(ResoursceId);

            return RedirectToAction("Edit", new { Id = ProjectId });
        }

        [Authorize()]
        [HttpPost]
        public IActionResult CreateConsumable(CreateConsumableViewModel createProjectView)
        {
            _projectService.CreateConsumable(createProjectView);

            return RedirectToAction("Edit", new { Id = createProjectView.ProjectId });

        }
        [Authorize()]
        [HttpPost]
        public IActionResult DeleteConsumable(Guid ConsumableId, Guid ProjectId)
        {
            _projectService.DeleteConsumable(ConsumableId);

            return RedirectToAction("Edit", new { Id = ProjectId });
        }

        [Authorize(Roles = Role.AllAdmin)]
        public IActionResult FindUsers(string Username, Guid ProjectId)
        {
            if (!string.IsNullOrEmpty(Username))
            {
                var userView = new List<UserViewModel>();
                Username = Username.ToLower();
                var users = _userManager.Users.Where(x => (x.Email.ToLower().Contains(Username) || x.UserName.ToLower().Contains(Username))).ToList();
                if (users.Count() != 0)
                {

                    userView = users.Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        RoleName = _userManager.GetRolesAsync(x).Result.First()
                    }).ToList();
                }
                TempData["Users"] = JsonConvert.SerializeObject(userView); 
            }
            return RedirectToAction("AddUser", ProjectId);
        }
    }
}