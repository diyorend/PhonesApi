namespace PhoneApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public Reviewer Reviewer { get; set; }
        public Phone Phone { get; set; }
    }
}
