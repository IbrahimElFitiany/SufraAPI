
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;
using Sufra.Exceptions;

namespace Sufra.Services.Services
{
    public class MenuSectionServices : IMenuSectionServices
    {

        private readonly IMenuSectionRepository _menuSectionRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public MenuSectionServices(IMenuSectionRepository menuSectionRepository, IRestaurantRepository restaurantRepository)
        {
            _menuSectionRepository = menuSectionRepository;
            _restaurantRepository = restaurantRepository;
        }

        //-------------------------------------------------------------------------------


        public async Task<CreateMenuSectionResDTO> CreateAsync(MenuSectionDTO menuSectionDTO)
        {
            bool? approved = await _restaurantRepository.GetRestaurantStatusByIdAsync(menuSectionDTO.RestaurantId);

      
            if(approved == null)
            {
                throw new RestaurantNotFoundException("restaurant Not found");
            }

            if (approved == false)
            {
                throw new RestaurantNotApprovedException("Restaurant not approved");
            }


            MenuSection menuSection = new MenuSection
            {
                RestaurantId = menuSectionDTO.RestaurantId,
                Name = menuSectionDTO.MenuSectionName
            };
            
            await _menuSectionRepository.CreateAsync(menuSection);
            return new CreateMenuSectionResDTO
            {
                Status = "success",
                MenuSection = menuSection.Name,
                RestaurantId = menuSection.RestaurantId
            };
        }

        public async Task DeleteAsync(MenuSectionDTO menuSectionDTO)
        {

            bool? approved = await _restaurantRepository.GetRestaurantStatusByIdAsync(menuSectionDTO.RestaurantId);

            if (approved == null)
            {
                throw new RestaurantNotFoundException("restaurant Not found");
            }

            if (approved == false)
            {
                throw new RestaurantNotApprovedException("Restaurant not approved");
            }

            MenuSection menuSection = await _menuSectionRepository.GetByIdAsync(menuSectionDTO.MenuSectionId);

            if (menuSection == null) {
                throw new MenuSectionNotFoundException("Menu section not found.");
            } 

            if (menuSection.RestaurantId != menuSectionDTO.RestaurantId)
            {
                throw new MenuSectionUnauthorizedAccessException("No Menu Section with this name assossiated with this restauratnt");
            }

            await _menuSectionRepository.DeleteAsync(menuSection);

        }

        public Task<ICollection<MenuSection>> GetByRestaurantIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(MenuSectionDTO menuSection)
        {
            var existingMenuSection = await _menuSectionRepository.GetByIdAsync(menuSection.MenuSectionId);

            if (existingMenuSection == null)
            {
                throw new MenuSectionNotFoundException($"Menu section with ID {menuSection.MenuSectionId} not found.");
            }

            if (existingMenuSection.RestaurantId != menuSection.RestaurantId)
            {
                throw new MenuSectionUnauthorizedAccessException("Unauthorized to access this resource");
            }

            existingMenuSection.Name = menuSection.MenuSectionName;

            await _menuSectionRepository.UpdateAsync(existingMenuSection);

        }
    }
}
