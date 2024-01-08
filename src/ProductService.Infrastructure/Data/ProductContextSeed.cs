using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using ProductService.Domain.Entites;

namespace ProductService.Infrastructure.Data;

public class ProductContextSeed
{
    public async Task SeedAsync(ProductServiceContext context, IHostEnvironment env, ILogger<ProductContextSeed> logger)
    {
        if (!env.IsDevelopment())
        {
            return;
        }

        AsyncRetryPolicy policy = CreatePolicy(logger, nameof(ProductContextSeed));

        await policy.ExecuteAsync(async () =>
        {
            using (context)
            {
                await context.Database.MigrateAsync();

                if (!await context.Products.AnyAsync())
                {
                    await context.Products.AddRangeAsync(
                        new Product(
                            name: "Reese's Peanut Butter Cups",
                            description: "These peanut butter cups candies are a sweet delight that you can bite, break, dunk or nibble on",
                            price: 7.50,
                            active: true),
                        new Product(
                            name: "Skittles",
                            description: "Taste the Rainbow—Original bold fruit flavors include orange, lemon, green apple, grape, and strawberry",
                            price: 6.99,
                            active: true),
                        new Product(
                            name: "M&M's",
                            description: "Multi-colored button-shaped chocolates",
                            price: 12.30,
                            active: true));
                }

                await context.SaveChangesAsync();
            }
        });
    }

    private static AsyncRetryPolicy CreatePolicy(ILogger<ProductContextSeed> logger, string prefix, int retries = 3)
    {
        return Policy.Handle<SqlException>().
            WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(
                        exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix,
                        exception.GetType().Name,
                        exception.Message,
                        retry,
                        retries);
                }
            );
    }
}
