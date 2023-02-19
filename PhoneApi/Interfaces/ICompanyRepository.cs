using PhoneApi.Models;

namespace PhoneApi.Interfaces
{
    public interface ICompanyRepository
    {
        ICollection<Company> GetCompanies();
        Company GetCompanyById(int id);
        ICollection<Model> GetModelsFromCompany(int id);
        bool CompanyExists(int id);
        //create
        bool CreateCompany(Company company);
        //update
        bool UpdateCompany(Company company);
        bool DeleteCompany(Company company);
        bool Save();
    }
}
