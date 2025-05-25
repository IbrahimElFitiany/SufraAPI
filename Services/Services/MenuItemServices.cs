using Sufra.Models.Restaurants;
using Sufra.DTOs;
using Sufra.Services.IServices;
using Sufra.Repositories.IRepositories;
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.Exceptions;

namespace Sufra.Services.Services
{
    public class MenuItemServices : IMenuItemServices
    {
		private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMenuSectionRepository _menuSectionRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public MenuItemServices(IMenuItemRepository menuItemRepository , IMenuSectionRepository menuSectionRepository , IRestaurantRepository restaurantRepository)
		{
			_menuItemRepository = menuItemRepository;
			_menuSectionRepository = menuSectionRepository;
            _restaurantRepository = restaurantRepository;
        }

		//--------------------------------------------

        public async Task<CreateMenuItemResDTO> CreateMenuItemAsync(MenuItemDTO menuItemDTO)
        {
            bool? restaurantIsApproved = await _restaurantRepository.GetRestaurantStatusByIdAsync(menuItemDTO.RestaurantId);

            if (restaurantIsApproved == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            if ( restaurantIsApproved == false)
            {
                throw new RestaurantNotApprovedException("restaurant Not Approved");
            }



            MenuSection existingMenuSection = await _menuSectionRepository.GetByIdAsync(menuItemDTO.MenuSectionId);


            if (existingMenuSection == null || existingMenuSection.RestaurantId != menuItemDTO.RestaurantId)
            {
                throw new Exception($"A menu Section with the ID '{menuItemDTO.MenuSectionId}' doesn't exist for this restaurant.");
            }

            MenuItem existingMenuItem = await _menuItemRepository.GetMenuItemByRestaurantAndNameAsync(menuItemDTO.RestaurantId, menuItemDTO.Name);

            if (existingMenuItem != null)
            {
                throw new Exception($"A menu item with the name '{menuItemDTO.Name}' already exists for this restaurant.");
            }

            MenuItem menuItem = new MenuItem
			{
				RestaurantId = menuItemDTO.RestaurantId,
				MenuSectionId = menuItemDTO.MenuSectionId,
				Name = menuItemDTO.Name,
				MenuItemImg = menuItemDTO.MenuItemImg,
				Description = menuItemDTO.Description,
				Price = menuItemDTO.Price,
				Availability = menuItemDTO.Availability,
			};

            await _menuItemRepository.CreateMenuItemAsync(menuItem);

            menuItemDTO.MenuItemId = menuItem.Id;

			return new CreateMenuItemResDTO
			{
				Status = "success",
				Message = "Menu Item Created",
				MenuItemDTO = menuItemDTO
			};

        }
        public async Task UpdateMenuItem( MenuItemDTO menuItemDTO)
        {
            // Check if the restaurant is approved wala l2
            bool? restaurantIsApproved = await _restaurantRepository.GetRestaurantStatusByIdAsync(menuItemDTO.RestaurantId);

            if (restaurantIsApproved == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            if (restaurantIsApproved == false)
            {
                throw new RestaurantNotApprovedException("Restaurant not approved.");
            }

            // Get the menu section
            MenuSection existingMenuSection = await _menuSectionRepository.GetByIdAsync(menuItemDTO.MenuSectionId);

            // Validate menu section
            if (existingMenuSection == null || existingMenuSection.RestaurantId != menuItemDTO.RestaurantId)
            {
                throw new Exception($"A menu section with the ID '{menuItemDTO.MenuSectionId}' doesn't exist for this restaurant.");
            }

            // Get the existing menu item by id
            MenuItem existingMenuItem = await _menuItemRepository.GetMenuItemByIdAsync(menuItemDTO.MenuItemId);

            if (existingMenuItem == null || existingMenuItem.RestaurantId != menuItemDTO.RestaurantId)
            {
                throw new Exception($"A menu item with the ID '{menuItemDTO.MenuItemId}' doesn't exist for this restaurant.");
            }

            // Update the values
            existingMenuItem.Name = menuItemDTO.Name;
            existingMenuItem.MenuItemImg = menuItemDTO.MenuItemImg;
            existingMenuItem.Description = menuItemDTO.Description;
            existingMenuItem.Price = menuItemDTO.Price;
            existingMenuItem.Availability = menuItemDTO.Availability;
            existingMenuItem.MenuSectionId = menuItemDTO.MenuSectionId;

            // Save changes
            await _menuItemRepository.UpdateMenuItemAsync(existingMenuItem);

        }

        public async Task RemoveMenuItemAsync(int menuItemId , int restaurantId)
        {
            bool? restaurantIsApproved = await _restaurantRepository.GetRestaurantStatusByIdAsync(restaurantId);

            if (restaurantIsApproved == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            if (restaurantIsApproved == false)
            {
                throw new RestaurantNotApprovedException("restaurant Not Approved");
            }

            MenuItem menuItemExists = await _menuItemRepository.GetMenuItemByIdAsync(menuItemId);
            if(menuItemExists == null)
            {
                throw new Exception("Menu Item doesn't exist");
            }
            if(menuItemExists.RestaurantId != restaurantId)
            {
                throw new Exception($"menu item with id: {menuItemId} doesn't exist");
            }

            await _menuItemRepository.DeleteMenuItemAsync(menuItemExists);

        }
    }
}
