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

        [HttpGet]
        public async Task<ActionResult<List<MenuItem>>> GetAllMenuItems()
        {
            var items = await _menuItemService.GetAllMenuItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItemById(string id)
        {
            var item = await _menuItemService.GetMenuItemByIdAsync(id);
            if (item == null) return NotFound($"Menu item with ID '{id}' not found or invalid ID.");
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMenuItem(MenuItem menuItem)
        {
            await _menuItemService.CreateMenuItemAsync(menuItem);
            return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItem.Id }, menuItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMenuItem(string id, MenuItem menuItem)
        {
            var existing = await _menuItemService.GetMenuItemByIdAsync(id);
            if (existing == null) return NotFound($"Menu item with ID '{id}' not found or invalid ID.");

            menuItem.Id = id;
            await _menuItemService.UpdateMenuItemAsync(id, menuItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenuItem(string id)
        {
            var existing = await _menuItemService.GetMenuItemByIdAsync(id);
            if (existing == null) return NotFound($"Menu item with ID '{id}' not found or invalid ID.");

            await _menuItemService.DeleteMenuItemAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/reviews")]
        public async Task<ActionResult> AddReview(string id, Review review)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null) return NotFound($"Menu item with ID '{id}' not found or invalid ID.");

            await _menuItemService.AddReviewAsync(id, review);
            return Ok();
        }

        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<List<Review>>> GetReviewsForMenuItem(string id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null) return NotFound($"Menu item with ID '{id}' not found or invalid ID.");

            var reviews = await _menuItemService.GetReviewsForMenuItemAsync(id);
            return Ok(reviews);
        }
    }
}
