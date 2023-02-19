namespace PhoneApi.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public Company Company { get; set; }
    }
}
