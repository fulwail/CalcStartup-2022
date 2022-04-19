using CalcStartup.Models;
using Microsoft.AspNetCore.Identity;

namespace CalcStartup.Identity.Model
{
    public class User : IdentityUser
    {
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
        public virtual List<ProjectUser> ProjectUsers { get; set; }

    }
}
