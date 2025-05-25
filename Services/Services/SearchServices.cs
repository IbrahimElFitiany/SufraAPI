using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DuoVia.FuzzyStrings;
using SufraMVC.Services.IServices;
using SufraMVC.Repositories.IRepositories;
using SufraMVC.DTOs;
using SufraMVC.Models.Restaurants;

namespace SufraMVC.Services.Services
{
    public class SearchServices : ISearchServices
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;


        public SearchServices(IRestaurantRepository restaurantRepository, IMenuItemRepository menuItemRepository)
        {
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
        }

        //--------------------------------------------------

        public async Task<IEnumerable<RestaurantDTO>> SearchRestaurantsAsync(string query)
        {

            IEnumerable<Restaurant> restaurants = await _restaurantRepository.GetAllAsync();

            string normalizedQuery = query?.Trim().ToLower();

            if (string.IsNullOrEmpty(normalizedQuery))
            {
                return restaurants.Where(r =>
                r.IsApproved == true).Select(r => new RestaurantDTO
                {
                    RestaurantId = r.Id,
                    ImgUrl = r.ImgUrl,
                    Name = r.Name,
                    Phone = r.Phone,
                    CuisineId = r.CuisineId,
                    CuisineName = r.Cuisine.Name,
                    Description = r.Description,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    Address = r.Address,
                    DistrictId = r.DistrictId,
                    Rating = r.Rating
                });
            }

            IEnumerable<Restaurant> fuzzyResults = restaurants.Where(r =>
                r.IsApproved == true &&
                (
                    r.Name != null && (r.Name.ToLower().Contains(normalizedQuery) ||
                    r.Name.FuzzyMatch(normalizedQuery) >= 0.3) ||

                    r.District?.Name != null && (r.District.Name.ToLower().Contains(normalizedQuery) ||
                    r.District.Name.FuzzyMatch(normalizedQuery) >= 0.3) ||

                    r.Cuisine?.Name != null && (r.Cuisine.Name.ToLower().Contains(normalizedQuery) ||
                    r.Cuisine.Name.FuzzyMatch(normalizedQuery) >= 0.3)
                )
            ).ToList();

            var result = fuzzyResults.Select(r => new RestaurantDTO
            {
                RestaurantId = r.Id,
                ImgUrl = r.ImgUrl,
                Name = r.Name,
                Phone = r.Phone,
                CuisineId = r.CuisineId,
                CuisineName =r.Cuisine.Name,
                Description = r.Description,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Address = r.Address,
                DistrictId = r.DistrictId,
                Rating = r.Rating
            });

            return result;
        }

    }
}
