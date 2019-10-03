using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Book>> GetBooks()
        {
            return await _context.Books.OrderBy(b => b.Title).ToListAsync();
        }

        public async Task<Book> GetBookById(int bookId)
        {
            return await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<Book> GetBookByIsbn(string isbn)
        {
            return await _context.Books.SingleOrDefaultAsync(b => b.Isbn == isbn);
        }

        public async Task<bool> BookExistsById(int bookId)
        {
            return await _context.Books.AnyAsync(b => b.Id == bookId);
        }

        public async Task<bool> BookExistsByIsbn(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.Isbn == isbn);
        }

        public async Task<bool> IsDuplicateIsbn(string isbn, int bookId)
        {
            return await _context.Books.AnyAsync(b => string.Equals(b.Isbn, isbn.Trim(), StringComparison.InvariantCultureIgnoreCase) && b.Id != bookId);

        }

        public async Task<decimal> GetBookRating(int bookId)
        {
            var reviews = await _context.Reviews.Where(b => b.Book.Id == bookId).ToListAsync();

            if (!reviews.Any())
                return 0;
            else
                return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }
    }
}
