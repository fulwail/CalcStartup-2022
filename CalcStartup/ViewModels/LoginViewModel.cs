using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CalcStartup.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email или Логин")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        [ValidateNever]
        public string ReturnUrl { get; set; }
    }
}
