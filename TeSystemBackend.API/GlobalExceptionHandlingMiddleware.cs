using TeSystemBackend.Application.DTOs;

namespace TeSystemBackend.API;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception occurred while processing the request.");

        var errorCode = exception switch
        {
            UnauthorizedAccessException => ErrorCodes.Unauthorized,
            KeyNotFoundException => ErrorCodes.NotFound,
            FluentValidation.ValidationException => ErrorCodes.ValidationFailed,
            ArgumentNullException => ErrorCodes.BadRequest,
            ArgumentException => ErrorCodes.BadRequest,
            InvalidOperationException => ErrorCodes.BadRequest,
            _ => ErrorCodes.InternalError
        };

        object? data = null;
        var message = exception.Message;

        if (exception is FluentValidation.ValidationException fvEx)
        {
            var errors = fvEx.Errors
                .Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage,
                    AttemptedValue = e.AttemptedValue
                })
                .ToList();

            if (errors.Count > 0)
            {
                message = errors[0].Error;
            }
            else
            {
                message = "Validation failed";
            }

            data = errors;
        }
        else if (exception is UnauthorizedAccessException unauthorizedEx)
        {
            if (unauthorizedEx.Message.Contains("username", StringComparison.OrdinalIgnoreCase))
            {
                data = new[]
                {
                    new
                    {
                        Field = "Username",
                        Error = unauthorizedEx.Message
                    }
                };
            }
            else if (unauthorizedEx.Message.Contains("password", StringComparison.OrdinalIgnoreCase))
            {
                data = new[]
                {
                    new
                    {
                        Field = "Password",
                        Error = unauthorizedEx.Message
                    }
                };
            }
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorCode;

        var response = ApiResponse<object>.Fail(errorCode, message);
        response.Data = data;

        await context.Response.WriteAsJsonAsync(response);
    }
}


