using BookApiApp.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int reviewId);
        Task<ICollection<Review>> GetReviewsOfABook(int bookId);
        Task<Book> GetBookOfAReview(int reviewId);
        Task<bool> ReviewExists(int reviewId);

    }
}
