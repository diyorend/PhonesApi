using PhoneApi.Data;
using PhoneApi.Interfaces;
using PhoneApi.Models;

namespace PhoneApi.Repository
{
    public class ModelRepository : IModelRepository
    {
        private readonly DataContext _context;

        public ModelRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateModel(Model model)
        {
            _context.Models.Add(model);
            return Save();
        }

        public bool DeleteModel(Model model)
        {
            _context.Models.Remove(model);
            return Save();
        }

        public bool DeleteModels(List<Model> models)
        {
            _context.Models.RemoveRange(models);
            return Save();
        }

        public Company GetCompanyByModel(int id)
        {
            return _context.Models.Where(m => m.Id == id).Select(m=> m.Company).FirstOrDefault();
        }

        public Model GetModel(int id)
        {
            return _context.Models.Where(m => m.Id == id).FirstOrDefault();
        }

        public ICollection<Model> GetModels()
        {
            return _context.Models.ToList();
        }

        public ICollection<Phone> GetPhonesFromModel(int id)
        {
            return _context.Phones.Where(p => p.Model.Id == id).ToList();
        }

        public bool ModelExists(int id)
        {
            return _context.Models.Any(m => m.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateModel(Model model)
        {
            _context.Models.Update(model);
            return Save();
        }
    }
}
