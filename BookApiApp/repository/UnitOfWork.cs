using BookApiApp.services;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class UnitOfWork : IWork
    {
        private readonly BookDbContext _context;

        public UnitOfWork(BookDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
            return await Save();
        }

        public async Task<bool> Update<T>(T entity) where T : class
        {
            _context.Update(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}