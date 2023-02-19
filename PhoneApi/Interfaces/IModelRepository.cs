using PhoneApi.Models;

namespace PhoneApi.Interfaces
{
    public interface IModelRepository
    {
        ICollection<Model> GetModels();
        Model GetModel(int id);
        Company GetCompanyByModel(int id);
        ICollection<Phone> GetPhonesFromModel(int id);
        bool ModelExists(int id);
        // create
        bool CreateModel(Model model);
        //update
        bool UpdateModel(Model model);
        bool DeleteModel(Model model);
        bool DeleteModels(List<Model> models);
        bool Save();
    }
}
