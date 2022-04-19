using CalcStartup.Domain;
using CalcStartup.Models;
using CalcStartup.ViewModels;

namespace CalcStartup.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ISqlContext _context;

        public CompanyService(ISqlContext context)
        {
            _context = context;
        }

        public void CreateCompany(CreateCompanyViewModel companyViewModel)
        {
            var company = new Company()
            { 
                Id = Guid.NewGuid(),
                Name = companyViewModel.Name
            };
          _context.Companies.Add(company);
          _context.SaveChanges();
        }

        public void DeleteCompany(Guid id)
        {
            var company = _context.Companies.FirstOrDefault(x => x.Id == id);
            if(company != null)
                _context.Companies.Remove(company);
            _context.SaveChanges();
        }
        public List<Company> GetCompanies()
        {
            return _context.Companies.ToList();
        }
        public Company GetCompanyById(Guid id)
        {
            return _context.Companies.FirstOrDefault(x => x.Id == id);
        }
    }     
}

