﻿namespace Sufra.DTOs.RestaurantDTOs.TableDTOs
{
    public class TableDTO
    {
        public int TableId { get; set; }
        public int RestaurantId { get; set; }
        public int Capacity { get; set; }
        public string Label { get; set; }
    }
}