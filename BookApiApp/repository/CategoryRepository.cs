using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _context;

        public CategoryRepository(BookDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<Category>> GetCategoriesOfABook(int id)
        {
            return await _context.BookCategories.Where(b => b.BookId == id).Select(c => c.Category).ToListAsync();
        }

        public async Task<ICollection<Book>> GetBooksForCategory(int id)
        {
            //return await _context.Books.Where(bc => bc.BookCategories.All(c => c.CategoryId == id)).ToListAsync();
            return await _context.BookCategories.Where(c => c.CategoryId == id).Select(b => b.Book).ToListAsync();
        }

        public async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IsCategoryDuplicate(string categoryName, int catId)
        {
            return await _context.Categories.AnyAsync(c => string.Equals(c.Name, categoryName.Trim(), StringComparison.InvariantCultureIgnoreCase) && c.Id != catId);
        }
    }
}
