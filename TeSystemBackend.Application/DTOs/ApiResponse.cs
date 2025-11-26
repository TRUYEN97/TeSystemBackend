namespace TeSystemBackend.Application.DTOs;

public class ApiResponse<T>
{
    public int ErrorCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            ErrorCode = 0,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(int errorCode, string message)
    {
        return new ApiResponse<T>
        {
            ErrorCode = errorCode,
            Message = message,
            Data = default
        };
    }
}


