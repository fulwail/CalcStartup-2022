using CalcStartup.Identity.Model;
using CalcStartup.Service;
using CalcStartup.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalcStartup.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly ICompanyService _companyService;

        public CompanyController(ILogger<ProjectController> logger, ICompanyService companyService)
        {
            _logger = logger;
            _companyService = companyService;
        }

        [Authorize(Roles =Role.GlobalAdmin)]
        public IActionResult Index()
        {
        
            var errorMessage = TempData["Message"] as string;
            var companies = _companyService.GetCompanies();
            var companiesView = new CompanyViewModel
            {
                Message = errorMessage,
                Companies = companies
            };
            return View(companiesView);
        
        }

        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            _companyService.DeleteCompany(id);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = Role.GlobalAdmin)]
        [HttpPost]
        public IActionResult Create(CreateCompanyViewModel createCompanyViewModel)
        {
            if (ModelState.IsValid)
            {
                _companyService.CreateCompany(createCompanyViewModel);
            }
            else
            {
                TempData["Message"] = "Все поля обязательны для заполнения";
            }

            return RedirectToAction("Index");

        }
    }
}