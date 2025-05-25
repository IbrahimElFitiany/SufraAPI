namespace Sufra.DTOs
{
    public class RestaurantLoginResponseDTO
    {
        public int ManagerID{ get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public bool IsApproved { get; set; }

        public string Token { get; set; }

    }
}