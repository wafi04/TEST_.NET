namespace CrudApi.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; } //  success 
    public string Message { get; set; } //  message api
    public T? Data { get; set; }  //  data props
    public object? Meta { get; set; } //  
    public List<string>? Errors { get; set; }

    public ApiResponse(bool success = true, string message = "", T? data = default, object? meta = null, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Meta = meta;
        Errors = errors;
    }

    public static ApiResponse<T> Ok(T? data, string message = "Success", object? meta = null)
    {
        return new ApiResponse<T>(true, message, data, meta);
    }

    public static ApiResponse<T> Fail(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>(false, message, errors: errors);
    }
}

public class PaginatedApiResponse<T> : ApiResponse<T>
{
    public PaginationMeta Pagination { get; set; }

    public PaginatedApiResponse(T? data, PaginationMeta pagination, string message = "Success") 
        : base(true, message, data)
    {
        Pagination = pagination;
    }
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
}