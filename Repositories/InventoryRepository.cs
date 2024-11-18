using ss_inventory_microservice.Models;
using ss_inventory_microservice.Data;
using MongoDB.Driver;

namespace ss_inventory_microservice.Repositories
{
    public interface IInventoryRepository
    {
        Task<InventoryItem> GetItemByIdAsync(string id);
        Task UpdateItemQuantityAsync(InventoryItem item);
    }
    public class InventoryRepository : IInventoryRepository
    {
        private readonly MongoDbContext _ctx;

        public InventoryRepository(MongoDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllAsync()
        {
            return await _ctx.InventoryItems.Find(_ => true).ToListAsync();
        }

        public async Task<InventoryItem> GetByIdAsync(string id)
        {
            return await _ctx.InventoryItems.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(InventoryItem item)
        {
            await _ctx.InventoryItems.InsertOneAsync(item);
        }

        public async Task UpdateAsync(string id, InventoryItem item)
        {
            await _ctx.InventoryItems.ReplaceOneAsync(i => i.Id == id, item);
        }

        public async Task DeleteAsync(string id)
        {
            await _ctx.InventoryItems.DeleteOneAsync(item => item.Id == id);
        }

        public async Task<InventoryItem> GetItemByIdAsync(string id)
        {
            return await _ctx.InventoryItems.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateItemQuantityAsync(InventoryItem item)
        {
            await _ctx.InventoryItems.ReplaceOneAsync(i => i.Id == item.Id, item);
        }
    }

    public class OrderMessage
    {
        public string? ProductId { get; set; }
        public int Quantity { get; set; }
    }
}