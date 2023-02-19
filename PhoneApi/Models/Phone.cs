namespace PhoneApi.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleasedDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Model Model { get; set; }
        public ICollection<PhoneCategory> PhoneCategories { get; set; }
    }
}
