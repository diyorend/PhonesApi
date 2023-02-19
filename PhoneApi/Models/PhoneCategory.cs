namespace PhoneApi.Models
{
    public class PhoneCategory
    {
        public int PhoneId { get; set; }
        public int CategoryId { get; set; }
        public Phone Phone { get; set; }
        public Category Category { get; set; }
    }
}
