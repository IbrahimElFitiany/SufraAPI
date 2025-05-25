namespace SufraMVC.DTOs
{
    public class RestaurantRegisterResponseDTO
    {
        public string message { get; set; }


        public int managerID { get; set; }
        public string managerFname { get; set; }


        public int restaurantID { get; set; }
        public string restaurantName { get; set; }
        public bool status { get; set; }

    }
}