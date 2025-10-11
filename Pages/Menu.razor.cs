
using Microsoft.AspNetCore.Components;
using RestaurantMS.Models;
using RestaurantMS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantMS.Pages
{
    public partial class Menu
    {
        private List<MenuItem>? _menuItems;
        private bool _showAddEditModal = false;
        private MenuItem _currentItem = new();
        private bool _isEditing = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadMenuItems();
        }

        private async Task LoadMenuItems()
        {
            _menuItems = await MenuService.GetMenuItemsAsync();
        }

        private void EditItem(MenuItem item)
        {
            _currentItem = item;
            _isEditing = true;
            _showAddEditModal = true;
        }

        private async Task DeleteItem(string? id)
        {
            if (id is not null)            
            {
                await MenuService.DeleteMenuItemAsync(id);
                await LoadMenuItems();
            }

        }

        private async Task HandleValidSubmit()
        {
            if (_isEditing)
            {
                if (_currentItem.Id is not null) await MenuService.UpdateMenuItemAsync(_currentItem.Id, _currentItem);
            }
            else
            {
                await MenuService.CreateMenuItemAsync(_currentItem);
            }

            _showAddEditModal = false;
            _isEditing = false;
            _currentItem = new(); // Reset for next use
            await LoadMenuItems();
        }
    }
}
