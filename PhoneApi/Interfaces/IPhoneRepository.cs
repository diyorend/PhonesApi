using PhoneApi.Models;

namespace PhoneApi.Interfaces
{
    public interface IPhoneRepository
    {
        ICollection<Phone> GetPhones();
        Phone GetPhone(int id);
        ICollection<Category> GetCategoriesOfPhone(int id);
        decimal GetPhoneRating(int id);
        ICollection<Review> GetReviewsOfPhone(int id);
        bool PhoneExists(int id);
        //create
        bool CreatePhone(int categoryId,Phone phone);
        // update
        bool UpdatePhone(int categoryId,Phone phone);
        bool DeletePhone(Phone phone);
        bool DeletePhones(List<Phone> phones);

        bool Save();
    }
}
