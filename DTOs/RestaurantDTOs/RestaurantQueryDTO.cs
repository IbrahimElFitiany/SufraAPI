public class RestaurantQueryDTO
{
    private int _page = 1;
    private int _pageSize = 10;

    public int? CuisineId { get; set; }
    public bool? IsApproved { get; set; }
    public int? DistrictId { get; set; }
     
    public string? Query { get; set; }
    public string? NormalizedQuery => Query?.Trim().ToLower().Replace(" ", "");

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            > 100 => 20,    
            < 10 => 10,
            _ => value
        };
    }
}