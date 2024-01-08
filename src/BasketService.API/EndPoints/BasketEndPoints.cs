using BasketService.API.Infrastructure.Interfaces;
using BasketService.API.Model;
using BasketService.API.Services;
using Carter;

namespace BasketService.API.EndPoints;

public class BasketEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/customer");

        group.MapGet("{id:guid}/baskets", GetBasketsById);
        //.WithName("GetProducts")
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Get List of Products",
        //    Description = "Returns information about products.",
        //    Tags = new List<OpenApiTag> { new() { Name = "Product" } }
        //});

        group.MapPost("{id:guid}/baskets", AddBasket);
        group.MapDelete("{id:guid}/baskets/{productId:int}", RemoveBasketItem);
    }

    public async Task<IResult> GetBasketsById(IBasketRepository repository, IProductService productService, Guid id)
    {
        CustomerBasket? basket = await repository.GetBasketAsync(id);

        return TypedResults.Ok(basket ?? new CustomerBasket(id, new List<Basket>()));
    }

    public async Task<IResult> AddBasket(IBasketRepository repository, IProductService productService, Guid id, BasketItem basketItem)
    {
        if (basketItem.Quantity <= 0)
        {
            return TypedResults.BadRequest("Invalid quantity");
        }

        CustomerBasket? basket = await repository.GetBasketAsync(id) ??
            new CustomerBasket(id, new List<Basket>());

        bool productAlreadyInBasket = basket.Items.Any(p => p.Id == basketItem.ProductId);

        if (productAlreadyInBasket)
        {
            basket.Items.First(p => p.Id == basketItem.ProductId).IncreaseQuantity(basketItem.Quantity);
        }
        else
        {
            Product? product = await productService.GetProduct(basketItem.ProductId);

            if (product is null)
            {
                return TypedResults.BadRequest("Product doest not exist");
            }

            basket.Items.Add(new Basket(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                basketItem.Quantity));
        }

        await repository.UpdateBasketAsync(basket);

        return TypedResults.Ok();
    }

    public async Task<IResult> RemoveBasketItem(IBasketRepository repository, IProductService productService, Guid id, int productId)
    {
        CustomerBasket? basket = await repository.GetBasketAsync(id);

        if (basket is null)
        {
            return TypedResults.Ok();
        }

        basket.Items.RemoveAll(a => a.Id == productId);

        await repository.UpdateBasketAsync(basket);

        return TypedResults.Ok();
    }
}
