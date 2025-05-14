using DTOs;
using DTOs.MenuSectionDTOs;
using DTOs.TableDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sufra.Controllers;
using Sufra_MVC.DTOs;
using Sufra_MVC.Exceptions;
using Sufra_MVC.Infrastructure.Services;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Repositories;
using Sufra_MVC.Services.IServices;

namespace Sufra_MVC.Services.Services
{
    public class RestaurantServices : IRestaurantServices
    {

        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantManagerRepository _restaurantManagerRepository;
        private readonly JwtServices _JwtService;


        public RestaurantServices(IRestaurantRepository restaurantRepository , IRestaurantManagerRepository restaurantManagerRepository , JwtServices jwtServices)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantManagerRepository = restaurantManagerRepository;
            _JwtService = jwtServices;
        }

        //----------------------------------------------------------------------------


        public async Task<RestaurantRegisterResponseDTO> RegistrationAsync(RestaurantRegisterRequestDTO restaurantRegistrationDTO)
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
        public async Task<RestaurantLoginResponseDTO> LoginAsync(RestaurantLoginRequestDTO restaurantLoginDTO)
        {
            try
            {

                RestaurantManager manager = await _restaurantManagerRepository.GetManagerByEmailAsync(restaurantLoginDTO.email);

                if (manager == null)
                    throw new Exception ("Invalid email or password. (For Testing: Email doens't exist)");

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(restaurantLoginDTO.password, manager.Password);

                if (!isPasswordValid)
                    throw new Exception ("Invalid email or password. (For Testing: password doens't match)");


                Restaurant restaurant = await _restaurantRepository.GetByManagerIdAsync(manager.Id);

                RestaurantClaimsDTO restaurantTokenDTO = new RestaurantClaimsDTO
                {
                    ManagerID = manager.Id,
                    ManagerName = manager.Fname,
                    Email = manager.Email,
                    RestaurantId = restaurant.Id,
                    RestaurantName = restaurant.Name,
                    IsApproved = restaurant.IsApproved,
                    Role = "Restaurant Manager"
                };
                string token = _JwtService.GenerateToken(restaurantTokenDTO);


                RestaurantLoginResponseDTO response = new RestaurantLoginResponseDTO
                {
                    ManagerID = manager.Id,
                    ManagerName = manager.Fname,
                    Email = manager.Email,
                    RestaurantId = restaurant.Id,
                    RestaurantName = restaurant.Name,
                    IsApproved = restaurant.IsApproved,
                    Token = token
                };

                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        //----------------------------------------------------------------------------
        public async Task ApproveRestaurantAsync(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("restaurant not found");
            }

            await _restaurantRepository.ApproveRestaurantById(restaurant);
        }
        public async Task BlockRestaurantAsync(int restaurantId)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("restaurant not found");
            }

            await _restaurantRepository.BlockRestaurantById(restaurant);
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
        public async Task<IEnumerable<RestaurantDTO>> GetAllAsync()
        {

            IEnumerable<Restaurant> restaurants = await _restaurantRepository.GetAllAsync();

            IEnumerable<RestaurantDTO> restaurantDtos = restaurants.Select(r => new RestaurantDTO
            {
                RestaurantId = r.Id,          // Assuming 'Id' is the PK in your Restaurant entity
                ImgUrl = r.ImgUrl,
                Name = r.Name,
                Phone = r.Phone,
                CuisineId = r.CuisineId,
                Description = r.Description,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Address = r.Address,
                DistrictId = r.DistrictId,
                IsApproved = r.IsApproved
            });

            return restaurantDtos;
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

        public async Task AddReviewAsync(int userId , CreateRestaurantReviewReqDTO reviewDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(reviewDTO.RestaurantId);

            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            if (restaurant.RestaurantReviews.Any(r => r.CustomerId == userId))
            {
                throw new InvalidOperationException("You have already reviewed this restaurant.");
            }

            RestaurantReview review = new RestaurantReview
            {
                CustomerId = userId,
                Rating = reviewDTO.Rating,
                ReviewDate = DateTime.SpecifyKind(reviewDTO.ReviewDate, DateTimeKind.Utc),
                RestaurantId = reviewDTO.RestaurantId,
                ReviewText = reviewDTO.Review
            };

            restaurant.AddReview(review);
            await _restaurantRepository.SaveAsync();

            restaurant.Rating = restaurant.RestaurantReviews.Average(r => r.Rating);

            await _restaurantRepository.SaveAsync();
        }
    }
}
