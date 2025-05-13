using DTOs;
using DTOs.TableDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using sufra.Controllers;
using Sufra_MVC.DTOs;
using Sufra_MVC.Models.RestaurantModels;

namespace Sufra_MVC.Services.IServices
{
    public interface IRestaurantServices
    {
        Task<RestaurantRegisterResponseDTO> RegistrationAsync(RestaurantRegisterRequestDTO restaurantRegistrationDTO);
        Task<RestaurantLoginResponseDTO> LoginAsync(RestaurantLoginRequestDTO restaurantLoginDTO);

        Task ApproveRestaurantAsync(int restaurantId);
        Task BlockRestaurantAsync(int restaurantId);
        Task<GetRestaurantResponseDTO> GetRestaurantAsync(int restaurantId);

        Task<CreateTableResDTO> AddTableAsync(TableDTO tableDTO);
        Task<IEnumerable<TableDTO>> GetAllTablesByRestaurantIdAsync(int restaurantId);
        Task RemoveTableAsync(int restaurantId, int tableId);

        Task AddOpeningHours(RestaurantOpeningHoursDTO restaurantOpeningHoursDTO);
        Task UpdateOpeningHours(RestaurantOpeningHoursDTO restaurantOpeningHoursDTO);
        Task DeleteOpeningHours(int RestaurantId, DayOfWeek dayOfWeek);


        Task AddReviewAsync(int userId, CreateRestaurantReviewReqDTO reviewDTO);
    }
}
