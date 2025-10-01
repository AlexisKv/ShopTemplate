namespace ShopTemplate.Helpers;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public List<string>? ErrorMessage { get; set; } 
    public T? Data { get; set; }
}