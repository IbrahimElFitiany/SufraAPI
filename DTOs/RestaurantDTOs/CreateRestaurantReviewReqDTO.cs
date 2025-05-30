﻿using System.ComponentModel.DataAnnotations;

namespace Sufra.DTOs.RestaurantDTOs
{
    public class CreateRestaurantReviewReqDTO
    {
        [Range(1,5)]
        public decimal Rating { get; set; }
        public string Review { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    }
}