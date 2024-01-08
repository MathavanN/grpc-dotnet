using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductServiceContext _context;
    private IProductRepository _product;
    public UnitOfWork(ProductServiceContext context)
    {
        _context = context;
        _product ??= new ProductRepository(_context);
    }


    public IProductRepository Product
    {
        get
        {
            _product ??= new ProductRepository(_context);
            return _product;
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}