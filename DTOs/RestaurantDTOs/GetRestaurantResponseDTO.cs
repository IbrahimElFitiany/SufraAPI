﻿using Sufra.DTOs.MenuSectionDTOs;

namespace Sufra.DTOs.RestaurantDTOs
{
    public class GetRestaurantResponseDTO
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Cuisine { get; set; }
        public string Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public decimal Rating { get; set; }

        public List<MenuSectionDTO> Menus { get; set; }
    }
}
