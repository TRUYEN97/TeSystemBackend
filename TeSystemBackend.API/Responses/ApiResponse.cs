using Microsoft.AspNetCore.Mvc;

namespace TeSystemBackend.API.Responses
{
    public record ApiResponse<T>(int errorCode, string message, T? data);

    public static class ApiResponse
    {
        public static ObjectResult Success<T>(T data, string message)
        {
            return Build(200, message, data);
        }

        public static ObjectResult Success<T>(T data)
        {
            return Build(200, "Thành công", data);
        }

        public static ObjectResult Fail<T>(int errorCode, string message, T? data = default)
        {
            return Build(errorCode, message, data);
        }

        public static ApiResponse<T> Body<T>(int errorCode, string message, T? data = default)
        {
            return new ApiResponse<T>(errorCode, message, data);
        }

        private static ObjectResult Build<T>(int errorCode, string message, T? data = default)
        {
            return new ObjectResult(Body(errorCode, message, data))
            {
                StatusCode = Normalize(errorCode)
            };
        }

        private static int Normalize(int errorCode)
        {
            return errorCode switch
            {
                200 or 400 or 401 or 404 or 500 => errorCode,
                _ => 500
            };
        }
    }
}

