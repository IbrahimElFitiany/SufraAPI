using Sufra.Models.Restaurants;
using Sufra.Services.IServices;
using Sufra.Repositories.IRepositories;
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.DTOs.MenuDTOs;
using Sufra.Exceptions.Restaurant;
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
                throw new NotFoundException<Restaurant>();
            }

            if ( restaurantIsApproved == false)
            {
                throw new RestaurantNotApprovedException();
            }

            MenuSection existingMenuSection = await _menuSectionRepository.GetByIdAsync(menuItemDTO.MenuSectionId);


            if (existingMenuSection == null) throw new NotFoundException<MenuSection>();

            if (existingMenuSection.RestaurantId != menuItemDTO.RestaurantId) throw new UnauthorizedException();

            MenuItem existingMenuItem = await _menuItemRepository.GetMenuItemByRestaurantAndNameAsync(menuItemDTO.RestaurantId, menuItemDTO.Name);

            if (existingMenuItem != null)
            {
                throw new AlreadyExistsException<MenuItem>($"A menu item with the name '{menuItemDTO.Name}' already exists for this restaurant.");
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
            bool? restaurantIsApproved = await _restaurantRepository.GetRestaurantStatusByIdAsync(menuItemDTO.RestaurantId);

            if (restaurantIsApproved == null) throw new NotFoundException<Restaurant>();
            if (restaurantIsApproved == false) throw new RestaurantNotApprovedException();


            MenuSection existingMenuSection = await _menuSectionRepository.GetByIdAsync(menuItemDTO.MenuSectionId);

            if (existingMenuSection == null) throw new NotFoundException<MenuSection>();
            if (existingMenuSection.RestaurantId != menuItemDTO.RestaurantId) throw new UnauthorizedException();

            MenuItem existingMenuItem = await _menuItemRepository.GetMenuItemByIdAsync(menuItemDTO.MenuItemId);

            if (existingMenuItem == null) throw new NotFoundException<MenuItem>();
            if (existingMenuItem.RestaurantId != menuItemDTO.RestaurantId) throw new UnauthorizedException();


            existingMenuItem.Name = menuItemDTO.Name;
            existingMenuItem.MenuItemImg = menuItemDTO.MenuItemImg;
            existingMenuItem.Description = menuItemDTO.Description;
            existingMenuItem.Price = menuItemDTO.Price;
            existingMenuItem.Availability = menuItemDTO.Availability;
            existingMenuItem.MenuSectionId = menuItemDTO.MenuSectionId;

            await _menuItemRepository.UpdateMenuItemAsync(existingMenuItem);

        }

        public async Task RemoveMenuItemAsync(int menuItemId , int restaurantId)
        {
            bool? restaurantIsApproved = await _restaurantRepository.GetRestaurantStatusByIdAsync(restaurantId);

            if (restaurantIsApproved == null) throw new NotFoundException<Restaurant>();

            if (restaurantIsApproved == false) throw new RestaurantNotApprovedException();


            MenuItem menuItemExists = await _menuItemRepository.GetMenuItemByIdAsync(menuItemId);

            if(menuItemExists == null) throw new NotFoundException<MenuItem>();

            if(menuItemExists.RestaurantId != restaurantId) throw new UnauthorizedException();


            await _menuItemRepository.DeleteMenuItemAsync(menuItemExists);
        }
    }
}
