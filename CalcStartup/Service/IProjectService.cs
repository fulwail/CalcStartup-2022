using CalcStartup.Identity.Model;
using CalcStartup.Models;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public interface IProjectService
    {
        List<Project> GetProjectsByUser(User user, bool isAdmin);
        Project GetProjectById(Guid? Id);
        void Delete(Guid Id);
        void Create(CreateProjectViewModel createProjectView, string userId);
        void DeleteUser(Guid userId, Guid projectId);
        void AddUser(Guid userId, Guid projectId);
        void Update(UpdateProjectViewModel project);
        void DeleteProjectWork(Guid id);
        void CreateProjectWork(CreateProjectWorkViewModel createProjectView);
        void CreateResource(CreateResourceViewModel createProjectView);
        void CreateStaff(CreateStaffViewModel createProjectView);
        void DeleteStaff(Guid staffId);
        void DeleteResource(Guid resoursceId);
        void DeleteConsumable(Guid consumableId);
        void CreateConsumable(CreateConsumableViewModel createProjectView);
        ReportViewModel GetStatistics(Guid id);
        ChartViewModelList GetBreakEvenVhart(Project project);
    }
}
