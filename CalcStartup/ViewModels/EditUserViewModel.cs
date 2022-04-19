using CalcStartup.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CalcStartup.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public Guid? CompanyId { get; set; }
        [ValidateNever]
        public List<Company> Companies { get; set; }
        public string Role { get; set; }     
    }
}
