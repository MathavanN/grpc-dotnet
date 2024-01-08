namespace BasketService.API.Model;
public record CustomerBasket(
Guid BuyerId,
List<Basket> Items)
{
    public double Total => Items.Sum(p => p.Total);
}
