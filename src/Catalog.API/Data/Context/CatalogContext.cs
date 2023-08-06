using Catalog.API.Entities;
using Catalog.API.Interfaces.Data.Context;
using MongoDB.Driver;

namespace Catalog.API.Data.Context;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        MongoClient client = new MongoClient(configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
        IMongoDatabase? database = client.GetDatabase(configuration.GetValue<string>("DatabaseSetting:Database"));
        Products =
            database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSetting:CollectionName"));
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}