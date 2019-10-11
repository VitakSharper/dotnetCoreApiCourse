using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookDbContext _context;

        public ReviewRepository(BookDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Review>> GetReviews()
        {
            return await _context.Reviews.OrderBy(r => r.Headline).ToListAsync();
        }

        public async Task<Review> GetReview(int reviewId)
        {
            return await _context.Reviews.SingleOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task<ICollection<Review>> GetReviewsOfABook(int bookId)
        {
            return await _context.Reviews.Where(b => b.Book.Id == bookId).ToListAsync();
        }

        public async Task<Book> GetBookOfAReview(int reviewId)
        {
            return await _context.Reviews.Where(r => r.Id == reviewId).Select(b => b.Book).SingleOrDefaultAsync();
        }

        public async Task<bool> ReviewExists(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }

    }
}
