namespace server.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public List<ApiError> Errors { get; set; } = new List<ApiError>();
    public bool HasErrors => Errors.Any();

    public ApiResponse(T? data)
    {
        Data = data;
    }

    public ApiResponse(List<ApiError> errors)
    {
        Errors = errors;
        Data = default;
    }
    
    public ApiResponse(T? data, List<ApiError> errors)
    {
        Data = data;
        Errors = errors ?? new List<ApiError>(); // Ensure errors never null
    }
}


public class ApiError
{
    public string Service { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Detail { get; set; } = string.Empty;
}
