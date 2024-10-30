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
		private readonly InventoryRepository _repository;

		public InventoryController(InventoryRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var items = await _repository.GetAllAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			var item = await _repository.GetByIdAsync(id);
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
			await _repository.CreateAsync(item);
			return CreatedAtAction(nameof(Get), new { id = Id }, item);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] InventoryItem item)
		{
			var existingItem = await _repository.GetByIdAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}
			await _repository.UpdateAsync(id, item);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var existingItem = await _repository.GetByIdAsync(id);
			if (existingItem == null)
			{
				return NotFound();
			}
			await _repository.DeleteAsync(id);
			return NoContent();
		}
	}
}