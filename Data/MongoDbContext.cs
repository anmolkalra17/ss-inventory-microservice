using ss_inventory_microservice.Models;
using MongoDB.Driver;

namespace ss_inventory_microservice.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase("InventoryDb");
    }

    public IMongoCollection<InventoryItem> InventoryItems => _database.GetCollection<InventoryItem>("InventoryItems");
}