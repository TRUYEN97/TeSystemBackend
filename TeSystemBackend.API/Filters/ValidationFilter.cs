using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using TeSystemBackend.API.Responses;

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
                        field = GetFieldName(x.Key),
                        message = e.ErrorMessage
                    }))
                    .ToList();

                context.Result = ApiResponse.Fail(400, "Dữ liệu không hợp lệ", errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private static string GetFieldName(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var segments = key.Split('.');
            return segments.Length > 0 ? segments[^1] : key;
        }
    }
}

