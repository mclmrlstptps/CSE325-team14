using Microsoft.AspNetCore.Mvc;
using RestaurantMS.Models;
using RestaurantMS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly MenuItemService _menuItemService;

        public MenuItemController(MenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // GET: api/MenuItem
        [HttpGet]
        public async Task<ActionResult<List<MenuItem>>> GetAllMenuItems()
        {
            var items = await _menuItemService.GetAllMenuItemsAsync();
            return Ok(items);
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id:length(24)}", Name = "GetMenuItem")]
        public async Task<ActionResult<MenuItem>> GetMenuItemById(string id)
        {
            var item = await _menuItemService.GetMenuItemByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/MenuItem
        [HttpPost]
        public async Task<ActionResult> CreateMenuItem([FromBody] MenuItem menuItem)
        {
            await _menuItemService.CreateMenuItemAsync(menuItem);
            return CreatedAtRoute("GetMenuItem", new { id = menuItem.Id }, menuItem);
        }

        // PUT: api/MenuItem/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateMenuItem(string id, [FromBody] MenuItem menuItem)
        {
            var existing = await _menuItemService.GetMenuItemByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _menuItemService.UpdateMenuItemAsync(id, menuItem);
            return NoContent();
        }

        // DELETE: api/MenuItem/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteMenuItem(string id)
        {
            var existing = await _menuItemService.GetMenuItemByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _menuItemService.DeleteMenuItemAsync(id);
            return NoContent();
        }

        // POST: api/MenuItem/{menuItemId}/review
        [HttpPost("{menuItemId:length(24)}/review")]
        public async Task<IActionResult> AddReview(string menuItemId, [FromBody] Review review)
        {
            await _menuItemService.AddReviewAsync(menuItemId, review);
            return Ok("Review added successfully.");
        }

        // GET: api/MenuItem/{menuItemId}/reviews
        [HttpGet("{menuItemId:length(24)}/reviews")]
        public async Task<ActionResult<List<Review>>> GetReviews(string menuItemId)
        {
            var reviews = await _menuItemService.GetReviewsForMenuItemAsync(menuItemId);
            return Ok(reviews);
        }
    }
}
