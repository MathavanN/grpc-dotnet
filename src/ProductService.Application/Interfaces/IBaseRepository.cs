using System.Linq.Expressions;

namespace ProductService.Application.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);
    Task<bool> Any(Expression<Func<T, bool>> filter);
}
