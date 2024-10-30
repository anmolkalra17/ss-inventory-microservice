using ss_inventory_microservice.Models;
using ss_inventory_microservice.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ss_inventory_microservice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly InventoryRepository _repo;

		public InventoryController(InventoryRepository repo)
		{
			_repo = repo;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var items = await _repo.GetAllAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var item = await _repo.GetByIdAsync(id);
			if (item == null)
			{
				return NotFound();
			}
			return Ok(item);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] InventoryItem item)
		{
            string Id = ObjectId.GenerateNewId().ToString();
			await _repo.CreateAsync(item);
			return CreatedAtAction(nameof(Get), new { id = Id }, item);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] InventoryItem item)
		{
			var existingItem = await _repo.GetByIdAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}
			await _repo.UpdateAsync(id, item);
			return Ok(new { message = "Item updated successfully" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var existingItem = await _repo.GetByIdAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}
			await _repo.DeleteAsync(id);
			return Ok(new { message = "Item deleted successfully" });
		}
	}
}