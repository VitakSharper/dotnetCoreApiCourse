using BookApiApp.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public interface IReviewerRepository
    {
        Task<ICollection<Reviewer>> GetReviewers();
        Task<Reviewer> GetReviewer(int id);
        Task<ICollection<Review>> GetReviewsByReviewer(int id);
        Task<Reviewer> GetReviewerOfAReview(int id);
        Task<bool> ReviewerExists(int id);
    }
}
