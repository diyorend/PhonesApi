using PhoneApi.Models;

namespace PhoneApi.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Phone> GetPhonesByCategory(int id);
        bool CategoryExists(int id);
        //create
        bool CreateCategory (Category category);
        //put
        bool UpdateCategory(Category category);
        //del
        bool DeleteCategory(Category category);
        bool Save();
    }
}
