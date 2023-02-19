using PhoneApi.Data;
using PhoneApi.Interfaces;
using PhoneApi.Models;

namespace PhoneApi.Repository
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly DataContext _context;

        public PhoneRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePhone(int categoryId, Phone phone)
        {
            var phoneCategoryEntity = _context.Categories.Where(a => a.Id== categoryId).FirstOrDefault();

            var phoneCategory = new PhoneCategory()
            {
                Category = phoneCategoryEntity,
                Phone = phone,
            };
            _context.PhoneCategories.Add(phoneCategory);
            _context.Phones.Add(phone);
            return Save();
        }

        public bool DeletePhone(Phone phone)
        {
            _context.Phones.Remove(phone);
            return Save();
        }

        public bool DeletePhones(List<Phone> phones)
        {
            _context.Phones.RemoveRange(phones);
            return Save();
        }

        public ICollection<Category> GetCategoriesOfPhone(int id)
        {
            return _context.PhoneCategories.Where(pc => pc.Phone.Id== id).Select(pc => pc.Category).ToList();
        }

        public Phone GetPhone(int id)
        {
            return _context.Phones.Where(p => p.Id == id).FirstOrDefault();
        }

       
        public decimal GetPhoneRating(int id)
        {
            var reviews = _context.Reviews.Where(p => p.Phone.Id == id);

            if (reviews.Count() <= 0)
                return 0;

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());

        }

        public ICollection<Phone> GetPhones()
        {
            return _context.Phones.OrderBy(p => p.Id).ToList();
        }

        public ICollection<Review> GetReviewsOfPhone(int id)
        {
            return _context.Reviews.Where(r => r.Phone.Id == id).ToList();
        }

        public bool PhoneExists(int id)
        {
            return _context.Phones.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePhone(int categoryId, Phone phone)
        {
            _context.Phones.Update(phone);
            return Save();
        }
    }
}
