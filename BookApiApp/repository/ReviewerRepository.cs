using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly BookDbContext _context;

        public ReviewerRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Reviewer>> GetReviewers()
        {
            return await _context.Reviewers.OrderBy(r => r.LastName).ToListAsync();
        }

        public async Task<Reviewer> GetReviewer(int id)
        {
            return await _context.Reviewers.SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ICollection<Review>> GetReviewsByReviewer(int id)
        {
            return await _context.Reviews.Where(r => r.Reviewer.Id == id).ToListAsync();
        }

        public async Task<Reviewer> GetReviewerOfAReview(int id)
        {
            return await _context.Reviews.Where(r => r.Id == id).Select(r => r.Reviewer).SingleOrDefaultAsync();
        }

        public async Task<bool> ReviewerExists(int id)
        {
            return await _context.Reviewers.AnyAsync(r => r.Id == id);
        }
    }
}