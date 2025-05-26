public class RestaurantQueryDTO
{
    public int? CuisineId { get; set; } //filtering will be implemented soon
    public bool? IsApproved { get; set; } //filtering will be implemented soon
    public int? DistrictId { get; set; } //filtering will be implemented soon

    public string? Query { get; set; }
    public string? NormalizedQuery => Query?.Trim().ToLower();

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}