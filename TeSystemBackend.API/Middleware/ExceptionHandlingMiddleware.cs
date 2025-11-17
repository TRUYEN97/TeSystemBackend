using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TeSystemBackend.API.Responses;
using TeSystemBackend.Service.Exceptions;

namespace TeSystemBackend.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var path = context.Request.Path.HasValue ? context.Request.Path.Value! : "/";
            var method = context.Request.Method;
            var exceptionName = exception.GetType().Name;

            _logger.LogError(
                exception,
                "Unhandled exception {ExceptionName} at {Method} {Path}: {Message}",
                exceptionName,
                method,
                path,
                exception.Message);

            var (errorCode, message) = exception switch
            {
                AppException appException => (appException.ErrorCode, appException.Message),
                _ => (500, "Lỗi hệ thống")
            };

            response.StatusCode = errorCode;
            var payload = ApiResponse.Body<object?>(errorCode, message, null);

            return response.WriteAsJsonAsync(payload);
        }
    }
}

