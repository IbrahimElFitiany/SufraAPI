namespace Sufra.DTOs.RestaurantDTOs
{
    public class UpdateRestaurantReqDTO
    {
        public string? ImgUrl { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? CuisineId { get; set; }
        public string? Description { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public bool? IsApproved { get; set; }
    }
}