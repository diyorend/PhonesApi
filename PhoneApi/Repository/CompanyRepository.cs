using PhoneApi.Data;
using PhoneApi.Dto;
using PhoneApi.Interfaces;
using PhoneApi.Models;

namespace PhoneApi.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _context;

        public CompanyRepository(DataContext context)
        {
            _context = context;
        }
        public bool CompanyExists(int id)
        {
            return _context.Companies.Any(c => c.Id == id);
        }

        public bool CreateCompany(Company company)
        {
            _context.Companies.Add(company);
            return Save();
        }

        public bool DeleteCompany(Company company)
        {
            _context.Companies.Remove(company);
            return Save();
        }

        public ICollection<Company> GetCompanies()
        {
            return _context.Companies.ToList();
        }

        public Company GetCompanyById(int id)
        {
            return _context.Companies.Where(c => c.Id == id).FirstOrDefault();
        }

        

        public ICollection<Model> GetModelsFromCompany(int id)
        {
            return _context.Models.Where(m => m.Company.Id == id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCompany(Company company)
        {
            _context.Companies.Update(company);
            return Save();
        }
    }
}
