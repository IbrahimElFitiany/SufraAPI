using Sufra.DTOs.RestaurantDTOs;
using Sufra.DTOs.RestaurantDTOs.OpeningHoursDTOs;
using Sufra.DTOs.RestaurantDTOs.TableDTOs;

namespace Sufra.Services.IServices
{
    public interface IRestaurantServices
    {
        Task<RestaurantRegisterResponseDTO> RegistrationAsync(RestaurantRegisterRequestDTO restaurantRegistrationDTO);
        Task<RestaurantLoginResponseDTO> LoginAsync(RestaurantLoginRequestDTO restaurantLoginDTO);

        Task ApproveRestaurantAsync(int restaurantId);
        Task BlockRestaurantAsync(int restaurantId);
        Task<GetRestaurantResponseDTO> GetRestaurantAsync(int restaurantId);
        Task<IEnumerable<RestaurantDTO>> GetSufraPicksAsync();
        Task<IEnumerable<RestaurantListItemDTO>> QueryRestaurantsAsync(RestaurantQueryDTO restaurantQueryDTO);
        Task DeleteAsync(int restaurantId);

        Task<CreateTableResDTO> AddTableAsync(TableDTO tableDTO);
        Task<IEnumerable<TableDTO>> GetAllTablesByRestaurantIdAsync(int restaurantId);
        Task RemoveTableAsync(int restaurantId, int tableId);
        Task UpdateRestaurantAsync(int restaurantId , UpdateRestaurantReqDTO updateRestaurantReqDTO);

        Task AddOpeningHours(RestaurantOpeningHoursDTO restaurantOpeningHoursDTO);
        Task UpdateOpeningHours(RestaurantOpeningHoursDTO restaurantOpeningHoursDTO);
        Task DeleteOpeningHours(int RestaurantId, DayOfWeek dayOfWeek);


        Task AddReviewAsync(int userId, CreateRestaurantReviewReqDTO reviewDTO);
    }
}
