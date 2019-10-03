using BookApiApp.services;
using System;
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
            var addEntity = await _context.AddAsync(entity);

            return
        }

        public Task<bool> Delete<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
