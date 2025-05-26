namespace Sufra.DTOs.RestaurantDTOs
{
    public class RestaurantManagerDTO
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
