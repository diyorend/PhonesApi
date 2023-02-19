using PhoneApi.Data;
using PhoneApi.Interfaces;
using PhoneApi.Models;

namespace PhoneApi.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);    
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.Reviews.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == id);
        }

        public Reviewer GetReviewerOfReview(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).Select(r => r.Reviewer).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(review => review.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            return Save();
        }

        
    }
}
