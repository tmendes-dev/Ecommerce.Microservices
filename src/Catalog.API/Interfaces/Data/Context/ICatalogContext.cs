using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Interfaces.Data.Context;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
    
}