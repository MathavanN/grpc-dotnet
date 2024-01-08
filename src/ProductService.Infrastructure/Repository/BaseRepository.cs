using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ProductServiceContext _context;
    internal DbSet<T> dbSet;
    private static readonly char[] separator = [','];

    public BaseRepository(ProductServiceContext context)
    {
        _context = context;
        dbSet = _context.Set<T>();
    }
    public void Add(T entity) => dbSet.Add(entity);

    public async Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            //case sensitive
            foreach (var property in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            //case sensitive
            foreach (var property in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        return await query.ToListAsync();
    }

    public void Remove(T entity) => dbSet.Remove(entity);

    public void Update(T entity) => dbSet.Update(entity);    

    public async Task<bool> Any(Expression<Func<T, bool>> filter)
    {
        return await dbSet.AnyAsync(filter);
    }
}
