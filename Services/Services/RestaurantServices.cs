using Microsoft.EntityFrameworkCore;
using Sufra.Common.Enums;
using Sufra.Common.Models;
using Sufra.DTOs;
using Sufra.DTOs.MenuDTOs;
using Sufra.DTOs.MenuSectionDTOs;
using Sufra.DTOs.RestaurantDTOs;
using Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs;
using Sufra.DTOs.RestaurantDTOs.TableDTOs;
using Sufra.Exceptions;
using Sufra.Infrastructure.Services;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;


namespace Sufra.Services.Services
{
    public class RestaurantServices : IRestaurantServices
    {

        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantManagerRepository _restaurantManagerRepository;
        private readonly ITokenService _tokenService;


        public RestaurantServices(IRestaurantRepository restaurantRepository , IRestaurantManagerRepository restaurantManagerRepository , ITokenService tokenService)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantManagerRepository = restaurantManagerRepository;
            _tokenService = tokenService;
        }

        //----------------------------------------------------------------------------


        public async Task<RestaurantRegisterResponseDTO> RegistrationAsync(RestaurantRegisterRequestDTO restaurantRegistrationDTO) //need to make it into single transaction
        {

            RestaurantManager existingManager = await _restaurantManagerRepository.GetManagerByEmailAsync(restaurantRegistrationDTO.RestaurantManager.Email);
            Restaurant existingRestaurant = await _restaurantRepository.GetByNameAsync(restaurantRegistrationDTO.Restaurant.Name);

            if (existingManager != null)
            {
                throw new EmailAlreadyInUseException("The email address is already in use.");
            }
            else if (existingRestaurant != null)
            {
                throw new RestaurantNameAlreadyInUseException($"Restaurant Name:'{restaurantRegistrationDTO.Restaurant.Name}' is already in use.");
            }

            restaurantRegistrationDTO.RestaurantManager.Password = BCrypt.Net.BCrypt.HashPassword(restaurantRegistrationDTO.RestaurantManager.Password);


            RestaurantManager restaurantManager = new RestaurantManager
            {
                Fname = restaurantRegistrationDTO.RestaurantManager.Fname,
                Lname = restaurantRegistrationDTO.RestaurantManager.Lname,
                Email = restaurantRegistrationDTO.RestaurantManager.Email,
                Password = restaurantRegistrationDTO.RestaurantManager.Password,
                RegistrationDate = DateTime.UtcNow
            };

            await _restaurantManagerRepository.AddManagerAsync(restaurantManager);


            Restaurant restaurant = new Restaurant
            {
                ImgUrl = restaurantRegistrationDTO.Restaurant.ImgUrl,
                Name = restaurantRegistrationDTO.Restaurant.Name,
                Phone = restaurantRegistrationDTO.Restaurant.Phone,
                CuisineId = restaurantRegistrationDTO.Restaurant.CuisineId,
                Description = restaurantRegistrationDTO.Restaurant.Description,
                Latitude = restaurantRegistrationDTO.Restaurant.Latitude,
                Longitude = restaurantRegistrationDTO.Restaurant.Longitude,
                Address = restaurantRegistrationDTO.Restaurant.Address,
                ManagerId = restaurantManager.Id,
                DistrictId = restaurantRegistrationDTO.Restaurant.DistrictId,
                IsApproved = false,
                Rating = 0

            };

            await _restaurantRepository.CreateAsync(restaurant);



            RestaurantRegisterResponseDTO response = new RestaurantRegisterResponseDTO
            {
                message = "Manager Registered , Restaurant Created , pending approval",

                managerID = restaurantManager.Id,
                managerFname = restaurantManager.Fname,

                restaurantID = restaurant.Id,
                restaurantName = restaurant.Name,
                status = restaurant.IsApproved
            };


            return response;
            
        }

        //----------------------------------------------------------------------------
        public async Task ApproveRestaurantAsync(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("restaurant not found");
            }
            if (restaurant.IsApproved)
            {
                throw new AlreadyApprovedException("Restaurant is already approved");
            }
            await _restaurantRepository.ApproveRestaurant(restaurant);
        }
        public async Task BlockRestaurantAsync(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("restaurant not found");
            }
            if (!restaurant.IsApproved)
            {
                throw new AlreadyBlockedException("Restaurant is already Blocked");
            }
            await _restaurantRepository.BlockRestaurant(restaurant);
        }

        public async Task<GetRestaurantResponseDTO> GetRestaurantAsync(int restaurantId)
        {
            try
            {
                Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

                if(restaurant.IsApproved == false)
                {
                    throw new Exception("Restaurant is not Approved in Sufra.");
                }

                return new GetRestaurantResponseDTO
                {
                    ImgUrl = restaurant.ImgUrl,
                    Name = restaurant.Name,
                    Phone = restaurant.Phone,
                    Cuisine = restaurant.Cuisine.Name,
                    Description = restaurant.Description,
                    Latitude = restaurant.Latitude,
                    Longitude = restaurant.Longitude,
                    Address = restaurant.Address,
                    District = restaurant.District.Name,
                    Rating = restaurant.Rating,
                    Menus = restaurant.MenuSections.Select(section => new MenuSectionDTO
                    {
                        RestaurantId = restaurant.Id,
                        MenuSectionId = section.Id,
                        MenuSectionName = section.Name,
                        Items = section.MenuItems.Select(item => new MenuItemDTO
                        {
                            MenuItemId = item.Id,
                            RestaurantId = restaurant.Id,
                            MenuSectionId = section.Id,
                            Name = item.Name,
                            MenuItemImg = item.MenuItemImg,
                            Description = item.Description,
                            Price = item.Price,
                            Availability = item.Availability
                        }).ToList()
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<PagedResultDTO<RestaurantListItemDTO>> QueryRestaurantsAsync(RestaurantQueryDTO restaurantQueryDTO)
        {

            PagedQueryResult<Restaurant> restaurants = await _restaurantRepository.QueryRestaurantsAsync(restaurantQueryDTO);

            PagedResultDTO<RestaurantListItemDTO> pagedResults = new PagedResultDTO<RestaurantListItemDTO>
            {
                Items = restaurants.Items.Select(r => new RestaurantListItemDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    Img = r.ImgUrl,
                    District = r.District.Name,
                    Gov = r.District.Gov.Name,
                    CuisineName = r.Cuisine.Name,
                    Rating = r.Rating
                }),
                Page = restaurants.Page,
                PageSize = restaurants.PageSize,
                TotalCount = restaurants.TotalCount,
                HasNextPage = restaurants.HasNextPage,
                HasPreviousPage = restaurants.HasPreviousPage,
            };

            return pagedResults;
        }


        public async Task DeleteAsync(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("restaurant not found");
            }

            await _restaurantRepository.DeleteRestaurant(restaurant);
        }

        public async Task UpdateRestaurantAsync(int restaurantId , UpdateRestaurantReqDTO updateRestaurantReqDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("restaurant not found");
            }

            if (updateRestaurantReqDTO.Name != null) restaurant.Name = updateRestaurantReqDTO.Name;
            if (updateRestaurantReqDTO.Phone != null) restaurant.Phone = updateRestaurantReqDTO.Phone;
            if (updateRestaurantReqDTO.ImgUrl != null) restaurant.ImgUrl = updateRestaurantReqDTO.ImgUrl;
            if (updateRestaurantReqDTO.CuisineId.HasValue) restaurant.CuisineId = updateRestaurantReqDTO.CuisineId.Value;
            if (updateRestaurantReqDTO.Description != null) restaurant.Description = updateRestaurantReqDTO.Description;
            if (updateRestaurantReqDTO.Latitude.HasValue) restaurant.Latitude = updateRestaurantReqDTO.Latitude.Value;
            if (updateRestaurantReqDTO.Longitude.HasValue) restaurant.Longitude = updateRestaurantReqDTO.Longitude.Value;
            if (updateRestaurantReqDTO.Address != null) restaurant.Address = updateRestaurantReqDTO.Address;
            if (updateRestaurantReqDTO.DistrictId.HasValue) restaurant.DistrictId = updateRestaurantReqDTO.DistrictId.Value;
            if (updateRestaurantReqDTO.IsApproved.HasValue) restaurant.IsApproved = updateRestaurantReqDTO.IsApproved.Value;

            await _restaurantRepository.UpdateRestaurant(restaurant);
        }

        public async Task<IEnumerable<RestaurantListItemDTO>> GetSufraPicksAsync()
        {

            IEnumerable<Restaurant> restaurants = await _restaurantRepository.GetSufraPicksAsync();

            IEnumerable<RestaurantListItemDTO> restaurantDtos = restaurants.Select(r => new RestaurantListItemDTO
            {
                Id=r.Id,
                Name=r.Name,
                Img=r.ImgUrl,
                Rating=r.Rating,
                District=r.District.Name,
                Gov=r.District.Gov.Name,
                CuisineName = r.Cuisine.Name,
            });

            return restaurantDtos;
        }

        //---------------------Table Services-----------------------------
        public async Task<CreateTableResDTO> AddTableAsync(TableDTO tableDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(tableDTO.RestaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            restaurant.AddTable(tableDTO.Capacity, tableDTO.Label);
            await _restaurantRepository.SaveAsync();
            return new CreateTableResDTO
            {
                message = "added"
            };
        }
        public async Task<IEnumerable<TableDTO>> GetAllTablesByRestaurantIdAsync(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            IEnumerable<Table> tables = restaurant.GetTables();

            IEnumerable<TableDTO> tableDTOs = tables.Select(t => new TableDTO
            {
                TableId = t.Id,
                RestaurantId = t.RestaurantId,
                Capacity = t.Capacity,
                Label = t.TableLabel
            });

            return tableDTOs;
        }
        public async Task RemoveTableAsync(int restaurantId, int tableId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            restaurant.RemoveTable(tableId);
            await _restaurantRepository.SaveAsync();
        }


        //---------------------Opening Hours Services-----------------------------

        public async Task AddOpeningHours(RestaurantOpeningHoursDTO restaurantOpeningHoursDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantOpeningHoursDTO.RestaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            restaurant.AddOpeningHour(restaurantOpeningHoursDTO.DayOfWeek, restaurantOpeningHoursDTO.OpenTime, restaurantOpeningHoursDTO.CloseTime);
            await _restaurantRepository.SaveAsync();
        }
        public async Task UpdateOpeningHours(RestaurantOpeningHoursDTO restaurantOpeningHoursDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantOpeningHoursDTO.RestaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            restaurant.UpdateOpeningHour(restaurantOpeningHoursDTO.DayOfWeek, restaurantOpeningHoursDTO.OpenTime, restaurantOpeningHoursDTO.CloseTime);
            await _restaurantRepository.SaveAsync();
        }
        public async Task DeleteOpeningHours(int RestaurantId , DayOfWeek dayOfWeek)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(RestaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            restaurant.DeleteOpeningHour(dayOfWeek);
            await _restaurantRepository.SaveAsync();

        }

        //---------------------Restaurant Review Services-----------------------------

        public async Task AddReviewAsync(int customerId ,int restaurantId , CreateRestaurantReviewReqDTO reviewDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant == null) throw new RestaurantNotFoundException("Restaurant not found.");

            if (restaurant.RestaurantReviews.Any(r => r.CustomerId == customerId)) throw new CustomerAlreadyReviewed("You have already reviewed this restaurant.");            

            RestaurantReview review = new RestaurantReview
            {
                CustomerId = customerId,
                Rating = reviewDTO.Rating,
                ReviewDate = DateTime.SpecifyKind(reviewDTO.ReviewDate, DateTimeKind.Utc),
                RestaurantId = restaurant.Id,
                ReviewText = reviewDTO.Review
            };

            restaurant.AddReview(review);
            await _restaurantRepository.SaveAsync();

            restaurant.Rating = restaurant.RestaurantReviews.Average(r => r.Rating);

            await _restaurantRepository.SaveAsync();
        }
    }
}
