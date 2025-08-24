
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;
using Sufra.Exceptions.Restaurant;
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

      
            if (approved == null) throw new NotFoundException<Restaurant>();
            if (approved == false) throw new RestaurantNotApprovedException();

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
                throw new NotFoundException<Restaurant>();
            }

            if (approved == false)
            {
                throw new RestaurantNotApprovedException();
            }

            MenuSection menuSection = await _menuSectionRepository.GetByIdAsync(menuSectionDTO.MenuSectionId);

            if (menuSection == null) {
                throw new NotFoundException<MenuSection>();
            } 

            if (menuSection.RestaurantId != menuSectionDTO.RestaurantId)
            {
                throw new UnauthorizedException();
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
                throw new NotFoundException<MenuSection>();
            }

            if (existingMenuSection.RestaurantId != menuSection.RestaurantId)
            {
                throw new UnauthorizedException();
            }

            existingMenuSection.Name = menuSection.MenuSectionName;

            await _menuSectionRepository.UpdateAsync(existingMenuSection);

        }
    }
}
