using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ss_inventory_microservice.Models;

public class InventoryItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}