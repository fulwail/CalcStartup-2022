using CalcStartup.Models;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public interface ICompanyService
    {
        List<Company> GetCompanies();
        void CreateCompany(CreateCompanyViewModel Company);
        void DeleteCompany(Guid Id);
        Company GetCompanyById(Guid guid);
    }
}
