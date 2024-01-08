using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entites;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repository;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly ProductServiceContext _context;

    public ProductRepository(ProductServiceContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<Product>> GetProductsAsync(int pageSize = 10, int pageIndex = 0)
    {
        return await _context.Products
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();
    }
}
