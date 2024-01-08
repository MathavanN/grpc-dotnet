using BasketService.API.Model;
using Grpc.Core;
using ProductServiceApi;

namespace BasketService.API.Services;

public class ProductService : IProductService
{
    private readonly ProductServiceGrpc.ProductServiceGrpcClient _client;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductServiceGrpc.ProductServiceGrpcClient client, ILogger<ProductService> logger)
    {
        _client = client;
        _logger = logger;
    }
    public async Task<Product?> GetProduct(int productId)
    {
        GetProductRequest request = new() { Id = productId };

        try
        {
            GetProductResponse response = await _client.GetProductAsync(request);

            return new Product(
                response.Id,
                response.Name,
                response.Description,
                response.Price);
        }
        catch (RpcException e)
        {
            _logger.LogWarning(e, "ERROR - Parameters: {@parameters}", request);

            return null;
        }
    }
}