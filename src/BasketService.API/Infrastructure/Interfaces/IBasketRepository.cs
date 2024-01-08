using BasketService.API.Model;

namespace BasketService.API.Infrastructure.Interfaces;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(Guid customerId);

    Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
}
