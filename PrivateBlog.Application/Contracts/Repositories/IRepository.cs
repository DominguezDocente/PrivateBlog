namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetListAsync();
        Task<int> GetTotalRecordsQuantity();
    }
}
