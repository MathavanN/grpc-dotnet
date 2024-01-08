using ProductService.Domain.Entites;

namespace ProductService.Application.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<List<Product>> GetProductsAsync(int pageSize = 10, int pageIndex = 0);
}
