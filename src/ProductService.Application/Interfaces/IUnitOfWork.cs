namespace ProductService.Application.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Product {  get; }
    Task CommitAsync();
}
