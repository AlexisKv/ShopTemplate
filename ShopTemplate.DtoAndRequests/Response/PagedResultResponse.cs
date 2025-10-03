namespace ShopTemplate.Dto.Response;

public class PagedResultResponse<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public List<T>? Items { get; set; }
}