using ss_inventory_microservice.Models;
using MongoDB.Driver;

namespace ss_inventory_microservice.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _db;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _db = client.GetDatabase("InventoryDb");
    }

    public IMongoCollection<InventoryItem> InventoryItems => _db.GetCollection<InventoryItem>("InventoryItems");
}