using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public Task<ICollection<Review>> GetReviews()
        {
            throw new NotImplementedException();
        }

        public Task<Review> GetReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Review>> GetReviewsOfABook(int bookId)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookOfAReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReviewExists(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }
    }
}
