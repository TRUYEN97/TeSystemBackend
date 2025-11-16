using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeSystemBackend.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => new
                    {
                        field = x.Key,
                        message = e.ErrorMessage
                    }))
                    .ToList();

                context.Result = new BadRequestObjectResult(new
                {
                    error = "Validation failed",
                    statusCode = 400,
                    errors = errors
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

