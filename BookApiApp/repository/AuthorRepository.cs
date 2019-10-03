using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _context;

        public AuthorRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Author>> GetAuthors()
        {
            return await _context.Authors.OrderBy(a => a.LastName).ToListAsync();
        }
        public async Task<Author> GetAuthor(int authorId)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Id == authorId);
        }
        public async Task<ICollection<Author>> GetAuthorsOfABook(int bookId)
        {
            return await _context.BookAuthors.Where(b => b.Book.Id == bookId).Select(a => a.Author).ToListAsync();
        }
        public async Task<ICollection<Book>> GetBooksByAuthor(int authorId)
        {
            return await _context.BookAuthors.Where(a => a.Author.Id == authorId).Select(b => b.Book).ToListAsync();
        }
        public async Task<bool> AuthorExists(int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.Id == authorId);
        }
    }
}
