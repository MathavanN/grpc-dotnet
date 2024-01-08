using Grpc.Core;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entites;
using ProductServiceApi;

namespace ProductService.API.Grpc;

public class GrpcProductService : ProductServiceGrpc.ProductServiceGrpcBase
{
    private readonly IUnitOfWork _unitOfWork;

    public GrpcProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        Product? product = await _unitOfWork.Product.Get(d => d.Id == request.Id);

        if (product is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Could not found product with id: {request.Id}"));
        }

        if (!product.Active)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with id: {request.Id} is disabled"));
        }

        return new GetProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        };
    }
}
