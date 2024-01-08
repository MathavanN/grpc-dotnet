using BasketService.API.Model;

namespace BasketService.API.Services;

public interface IProductService
{
    Task<Product?> GetProduct(int productId);
}
