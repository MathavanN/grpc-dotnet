using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProductService.Application.Interfaces;

namespace ProductService.API.EndPoints;

public class ProductEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/product");

        group.MapGet("", GetProducts)
            .WithName("GetProducts")
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get List of Products",
                Description = "Returns information about products.",
                Tags = new List<OpenApiTag> { new() { Name = "Product" } }
            });
    }

    public async Task<IResult> GetProducts(IUnitOfWork unitOfWork, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
    {
        var result = await unitOfWork.Product.GetProductsAsync(pageSize, pageIndex);
        return TypedResults.Ok(result);
    }
}
