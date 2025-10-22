namespace MillionPropertyApi.Modules.Properties.DTOs;

public class PropertyFilterDto
{
    public string? Query { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}

public class PaginatedPropertiesDto
{
    public List<PropertyDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
