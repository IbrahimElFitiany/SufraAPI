namespace Sufra.DTOs.RestaurantDTOs
{
    public class RestaurantLoginResponseDTO
    {
        public int ManagerID{ get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public bool IsApproved { get; set; }
    }
}