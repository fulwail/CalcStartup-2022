using CalcStartup.Identity.Model;
using CalcStartup.Models;
using CalcStartup.Service;
using CalcStartup.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CalcStartup.Controllers
{
    public class UserController : Controller
    {

        private readonly UserManager<User> _userManager;

        private readonly ICompanyService _companyService;
        public UserController(UserManager<User> userManager, ICompanyService companyService)
        {
            _userManager = userManager;
            _companyService = companyService;
        }
        [Authorize(Roles = Role.AllAdmin)]
        public IActionResult Index() 
        {
            var users = _userManager.Users.ToList();
            var usersViewModel = users.Select(x => new UserViewModel {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                RoleName = _userManager.GetRolesAsync(x).Result.FirstOrDefault(),
                CompanyName = _companyService.GetCompanyById(x.CompanyId?? Guid.Empty)?.Name?? String.Empty,
            }).ToList();
          return  View(usersViewModel);
        }

        [Authorize(Roles = Role.AllAdmin)]
        public IActionResult Create()
        {
            var createUser = new CreateUserViewModel();     

            createUser.Companies = GetCompanies();
            return View(createUser);
        }

        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Username,
                    CompanyId = model.CompanyId,
                };
                var createResult = await _userManager.CreateAsync(user, model.Password);
                var roleResult=  await _userManager.AddToRoleAsync(user,model.Role);
                if (createResult.Succeeded&& roleResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var errors = new List<IdentityError>();
                    errors.AddRange(createResult.Errors);
                    errors.AddRange(roleResult.Errors);   
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return View(model);
        }
        [Authorize(Roles = Role.AllAdmin)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var role = await _userManager.GetRolesAsync(user);
      
            var model = new EditUserViewModel {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                CompanyId = user.CompanyId,
                Companies = GetCompanies(),
                Role = role.FirstOrDefault()
            };
            return View(model);
        }

        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    user.Email = model.Email;
                    user.UserName = model.Username;
                    user.CompanyId = model.CompanyId;
                    var roleResult= await _userManager.AddToRoleAsync(user, model.Role);
                    var result = await _userManager.UpdateAsync(user);
                   
                    if (result.Succeeded&&roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            model.Companies=GetCompanies();
            return View(model);
        }

        [Authorize(Roles = Role.AllAdmin)]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        private List<Company> GetCompanies()
        {
            var companies = _companyService.GetCompanies();

            if (User.IsInRole(Role.TeamManager))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                companies = companies.Where(x => x.Id == user.CompanyId).ToList();
            }
            return companies;
        }

    }
}

