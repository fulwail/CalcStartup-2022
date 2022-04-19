using System.ComponentModel;

namespace CalcStartup.Identity.Model
{
    public static class Role
    {
        [Description("Администратор")]
        public const string GlobalAdmin = "Administrator";
        public const string TeamManager = "TeamManager";
        public const string Employee = "Employee";
        public const string AllAdmin = $"{GlobalAdmin},{TeamManager}";
    }
}
