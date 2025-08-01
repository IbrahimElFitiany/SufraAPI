﻿using Sufra.DTOs.RestaurantDTOs;

namespace Sufra.DTOs
{
    public class PagedResultDTO<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

    }
}
