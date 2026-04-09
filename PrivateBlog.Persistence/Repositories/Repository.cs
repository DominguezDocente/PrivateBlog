using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

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
            return Task.FromResult(entity);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            return Task.FromResult(entity);
        }
    }
}
