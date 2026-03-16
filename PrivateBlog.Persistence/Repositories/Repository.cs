using PrivateBlog.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PrivateBlog.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public Task<T> CreateAsync(T entity)
        {
            _context.Add(entity);
            return Task.FromResult(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int> GetTotalRecordsQuantity()
        {
            return await _context.Set<T>().CountAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.CompletedTask;
        }
    }
}
