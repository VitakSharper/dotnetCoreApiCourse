using BookApiApp.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public interface IAuthorRepository
    {
        Task<ICollection<Author>> GetAuthors();
        Task<Author> GetAuthor(int authorId);
        Task<ICollection<Author>> GetAuthorsOfABook(int bookId);
        Task<ICollection<Book>> GetBooksByAuthor(int authorId);
        Task<bool> AuthorExists(int authorId);
    }
}
