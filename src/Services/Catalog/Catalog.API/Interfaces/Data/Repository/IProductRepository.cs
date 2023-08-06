using Catalog.API.Entities;

namespace Catalog.API.Interfaces.Data.Repository;

public interface IProductRepository
{
    Task Create(Product product);
    Task<bool> Delete(string id);
    Task<bool> Update(Product product);
    Task<Product> GetById(string id);
    Task<IEnumerable<Product>> GetAll();
    Task<IEnumerable<Product>> GetByName(string name);
    Task<IEnumerable<Product>> GetByCategory(string categoryName);
}