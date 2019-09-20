using BookApiApp.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public interface IBookRepository
    {
        Task<ICollection<Book>> GetBooks();
        Task<Book> GetBookById(int bookId);
        Task<Book> GetBookByIsbn(string isbn);
        Task<bool> BookExistsById(int bookId);
        Task<bool> BookExistsByIsbn(string isbn);
        Task<bool> IsDuplicateIsbn(string isbn, int bookId);
        Task<int> GetBookRating(int bookId);
    }
}
