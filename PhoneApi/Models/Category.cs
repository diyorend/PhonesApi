namespace PhoneApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public ICollection<PhoneCategory> PhoneCategories { get; set; }
    }
}
