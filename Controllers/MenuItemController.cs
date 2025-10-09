using Microsoft.AspNetCore.Mvc;
using RestaurantMS.Models;
using RestaurantMS.Services;
using System;
using System.Threading.Tasks;

namespace RestaurantMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly MongoDBService _mongoService;

        public MenuItemController(MongoDBService mongoService)
        {
            _mongoService = mongoService;
        }

        // GET: api/MenuItem
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var menuItems = await _mongoService.GetMenuItemsAsync();
            return Ok(menuItems);
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var menuItem = await _mongoService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound(new { message = $"MenuItem with ID {id} not found." });

            return Ok(menuItem);
        }

        // POST: api/MenuItem
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuItem menuItem)
        {
            if (menuItem == null)
                return BadRequest(new { message = "MenuItem cannot be null." });

            await _mongoService.CreateMenuItemAsync(menuItem);
            return CreatedAtAction(nameof(GetById), new { id = menuItem.Id }, menuItem);
        }

        // PUT: api/MenuItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] MenuItem menuItem)
        {
            if (menuItem == null)
                return BadRequest(new { message = "MenuItem cannot be null." });

            var existing = await _mongoService.GetMenuItemByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"MenuItem with ID {id} not found." });

            menuItem.Id = id;
            await _mongoService.UpdateMenuItemAsync(id, menuItem);

            return NoContent();
        }

        // DELETE: api/MenuItem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _mongoService.GetMenuItemByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"MenuItem with ID {id} not found." });

            await _mongoService.DeleteMenuItemAsync(id);
            return NoContent();
        }

        // POST: api/MenuItem/{id}/review
        [HttpPost("{id}/review")]
        public async Task<IActionResult> AddReview(string id, [FromBody] Review review)
        {
            if (review == null)
                return BadRequest(new { message = "Review cannot be null." });

            var menuItem = await _mongoService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound(new { message = $"MenuItem with ID {id} not found." });

            review.CreatedAt = DateTime.UtcNow;
            await _mongoService.AddReviewToMenuItemAsync(id, review);

            return Ok(review);
        }

        // GET: api/MenuItem/{id}/reviews
        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviews(string id)
        {
            var menuItem = await _mongoService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound(new { message = $"MenuItem with ID {id} not found." });

            return Ok(menuItem.Reviews);
        }
    }
}
