using Carter;
using Microsoft.OpenApi.Models;

namespace ProductService.API.EndPoints;

public class AboutEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/about");

        group.MapGet("", GetAbout)
            .WithName("about")
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get Product Service Version Details",
                Description = "Returns information about Product Service.",
                Tags = new List<OpenApiTag> { new() { Name = "About" } }
            });
    }

    public static IResult GetAbout()
    {
        return TypedResults.Ok(new { data = "Product Service v1.0.0" });
    }
}
